using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using OpenXmlPowerTools;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SANS.Common.OpenXml
{
    public class WordOperateHelper
    {
        private WordprocessingDocument objWordDocument;
        public WordOperateHelper(string filePath)
        {
            objWordDocument = WordprocessingDocument.Open(filePath, true);
        }
        public void WordTextReplacer(string key, string value)
        {
            TextReplacer.SearchAndReplace(objWordDocument, key, value, true);
        }
        public void WordParagraphReplacer(string key, string value)
        {
            Body body = objWordDocument.MainDocumentPart.Document.Body;
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                if (paragraph.InnerText.IndexOf(key) >= 0)
                {
                    var t = paragraph.InnerText;
                    string modifiedOuterxml = Regex.Replace(paragraph.OuterXml, key, value);
                    OpenXmlElement parent = paragraph.Parent;
                    Paragraph modifiedParagraph = new Paragraph(modifiedOuterxml);
                    parent.ReplaceChild<Paragraph>(modifiedParagraph, paragraph);
                }
            }
        }
        public void WordRunReplacer(string key, string value)
        {
            Body body = objWordDocument.MainDocumentPart.Document.Body;
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                if (paragraph.InnerText.IndexOf(key) >= 0)
                {
                    var t = paragraph.InnerText;
                    string modifiedString = Regex.Replace(t, key, value);
                    paragraph.RemoveAllChildren<Run>();
                    paragraph.AppendChild<Run>(new Run(new Text(modifiedString)));
                }
            }
            foreach (var table in body.Elements<Table>())
            {
                foreach (var tableRow in table.Elements<TableRow>())
                {
                    var ttr = false;
                    foreach (var tableCell in tableRow.Elements<TableCell>())
                    {
                        if (tableCell.InnerText.IndexOf(key) >= 0)
                        {
                            ttr = true;
                            break;
                        }
                    }
                    if (ttr)
                    {
                        table.RemoveChild(tableRow);
                    }
                }
            }
        }
        public List<string> SearchWordText(string bg,string end)
        {
            Body body = objWordDocument.MainDocumentPart.Document.Body;
            List<string> ls = new List<string>();
            foreach (var inst in body.Elements<Paragraph>())
            {
                var l=StringHelper.ExtractStringBetweenBeginAndEnd(inst.InnerText, bg, end);
                if(l!=null&&l.Count>0)
                {
                    ls.AddRange(l);
                }
            }
            foreach (var table in body.Elements<Table>())
            {
                foreach (var tableRow in table.Elements<TableRow>())
                {
                    foreach (var tableCell in tableRow.Elements<TableCell>())
                    {
                        var l = StringHelper.ExtractStringBetweenBeginAndEnd(tableCell.InnerText, bg, end);
                        if (l != null && l.Count > 0)
                        {
                            ls.AddRange(l);
                        }
                    }
                }
            }
            return ls;
        }
        public void InsertImage(string picturePath, string SearchString)
        {
            Body body = objWordDocument.MainDocumentPart.Document.Body;
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                if (paragraph.InnerText.IndexOf(SearchString) >= 0)
                {
                    var t = paragraph.InnerText;
                    string picType = picturePath.Split('.').Last();
                    ImagePartType imagePartType;
                    ImagePart imagePart = null;
                    // 通过后缀名判断图片类型, true 表示忽视大小写
                    if (Enum.TryParse<ImagePartType>(picType, true, out imagePartType))
                    {
                        imagePart = objWordDocument.MainDocumentPart.AddImagePart(imagePartType);
                    }
                    imagePart.FeedData(File.Open(picturePath, FileMode.Open));
                    var relationshipId = objWordDocument.MainDocumentPart.GetIdOfPart(imagePart);
                    AddImageToBody(paragraph, relationshipId);
                }
            }
        }
        public void InsertTable(ReportTable rt, string SearchString)
        {
            Body body = objWordDocument.MainDocumentPart.Document.Body;
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                if (paragraph.InnerText.IndexOf(SearchString) >= 0)
                {
                    var t = paragraph.InnerText;
                    paragraph.Append(GetParagraph(rt, 1000));
                }
            }
        }
        public void UpdateTable(ReportTable rt, string SearchString)
        {
            Body body = objWordDocument.MainDocumentPart.Document.Body;
            foreach (var table in body.Elements<Table>())
            {
                foreach (var tableRow in table.Elements<TableRow>())
                {
                    var ttr = false;
                    foreach (var tableCell in tableRow.Elements<TableCell>())
                    {
                        var t = tableCell.InnerText;
                        if (t.IndexOf(SearchString) >= 0)
                        {
                            ttr = true;
                            break;
                        }
                    }
                    if (ttr)
                    {
                        table.RemoveChild(tableRow);
                        SetTableParagraph(rt, table);
                    }
                }
            }
        }
        public void SaveDoc()
        {
            objWordDocument.Save();
            objWordDocument.Close();
            objWordDocument.Dispose();
            
        }
        private  Text CreateText(string text)
        {
            if (text == null)
                text = string.Empty;
            Text t = new Text(text);
            if (text.EndsWith(" "))
            {
                t.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Preserve);
            }
            if (text.StartsWith(" "))
            {
                t.Space = new EnumValue<SpaceProcessingModeValues>(SpaceProcessingModeValues.Default);
            }
            return t;
        }
        private void SetTableParagraph(ReportTable reportTable, Table table)
        {
            int count = reportTable.Value.Count;
            int cols = reportTable.Column;
            int j = 0;
            foreach (List<string> strs in reportTable.Value)
            {
                TableRow row = new TableRow();
                for (int i = 0; i < cols; i++)
                {
                    TableCell cell = new TableCell();
                    TableCellProperties tableCellProperties = new TableCellProperties();
                    TableCellMargin margin = new TableCellMargin();
                    margin.LeftMargin = new LeftMargin() { Width = "100", Type = TableWidthUnitValues.Dxa };
                    margin.RightMargin = new RightMargin() { Width = "100", Type = TableWidthUnitValues.Dxa };
                    tableCellProperties.Append(margin);
                    Paragraph par = new Paragraph();
                    Run run = new Run();
                    if (j == 0 && reportTable.IsHaveColumn)
                    {
                        RunProperties rPr = new RunProperties();
                        rPr.Append(new Bold());
                        run.Append(rPr);
                    }
                    if (strs.Count != cols && i >= strs.Count - 1)
                    {
                        HorizontalMerge verticalMerge = new HorizontalMerge();
                        if (i == strs.Count - 1)
                        {
                            RunProperties rPr = new RunProperties();
                            rPr.Append(new Bold());
                            run.Append(rPr);
                            verticalMerge.Val = MergedCellValues.Restart;
                            run.Append(CreateText(strs[i]));
                        }
                        else
                        {
                            verticalMerge.Val = MergedCellValues.Continue;
                        }
                        tableCellProperties.Append(verticalMerge);
                    }
                    else
                    {
                        run.Append(CreateText(strs[i]));
                    }
                    par.Append(run);
                    cell.Append(tableCellProperties);
                    cell.Append(par);
                    row.Append(cell);
                }
                j++;
                table.Append(row);
            }
        }
        private Table GetParagraph(ReportTable reportTable, Int32Value width)
        {
            Table table = new Table();
            TableProperties tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 },
                    new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 }
                 )
            );
            tblProp.TableWidth = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            TableGrid tableGrid1 = new TableGrid();
            GridColumn gridColumn1 = new GridColumn() { Width = width.ToString() };
            GridColumn gridColumn2 = new GridColumn() { Width = width.ToString() };
            GridColumn gridColumn3 = new GridColumn() { Width = width.ToString() };
            tableGrid1.Append(gridColumn1);
            tableGrid1.Append(gridColumn2);
            tableGrid1.Append(gridColumn3);

            table.Append(tblProp);
            table.Append(tableGrid1);

            int count = reportTable.Value.Count;
            int cols = reportTable.Column;
            int j = 0;
            foreach (List<string> strs in reportTable.Value)
            {
                TableRow row = new TableRow();
                for (int i = 0; i < cols; i++)
                {
                    TableCell cell = new TableCell();
                    TableCellProperties tableCellProperties = new TableCellProperties();
                    TableCellMargin margin = new TableCellMargin();
                    margin.LeftMargin = new LeftMargin() { Width = "100", Type = TableWidthUnitValues.Dxa };
                    margin.RightMargin = new RightMargin() { Width = "100", Type = TableWidthUnitValues.Dxa };
                    tableCellProperties.Append(margin);
                    Paragraph par = new Paragraph();
                    Run run = new Run();
                    if (j == 0 && reportTable.IsHaveColumn)
                    {
                        Shading shading1 = new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "AEAAAA", ThemeFill = ThemeColorValues.Background2, ThemeFillShade = "BF" };
                        RunProperties rPr = new RunProperties();
                        tableCellProperties.Append(shading1);
                        rPr.Append(new Bold());
                        run.Append(rPr);
                    }
                    if (strs.Count != cols && i >= strs.Count - 1)
                    {
                        HorizontalMerge verticalMerge = new HorizontalMerge();
                        if (i == strs.Count - 1)
                        {
                            RunProperties rPr = new RunProperties();
                            rPr.Append(new Bold());
                            run.Append(rPr);
                            verticalMerge.Val = MergedCellValues.Restart;
                            run.Append(CreateText(strs[i]));
                        }
                        else
                        {
                            verticalMerge.Val = MergedCellValues.Continue;
                        }
                        tableCellProperties.Append(verticalMerge);
                    }
                    else
                    {
                        run.Append(CreateText(strs[i]));
                    }
                    par.Append(run);
                    cell.Append(tableCellProperties);
                    cell.Append(par);
                    row.Append(cell);
                }
                j++;
                table.Append(row);
            }

            return table;
        }
        private  void AddImageToBody(Paragraph wordDocp, string relationshipId)
        {

            // Define the reference of the image.
            var element =

                      new Drawing(
                      new DW.Inline(
                      new DW.Extent() { Cx = 5248275L, Cy = 2895600L }, // 调节图片大小
                                   new DW.EffectExtent()
                                   {
                                       LeftEdge = 0L,
                                       TopEdge = 0L,
                                       RightEdge = 0L,
                                       BottomEdge = 0L
                                   },
                      new DW.DocProperties()
                      {
                          Id = (UInt32Value)1U,
                          Name = "Picture 1"
                      },
                      new DW.NonVisualGraphicFrameDrawingProperties(
                      new A.GraphicFrameLocks() { NoChangeAspect = true }),
                      new A.Graphic(
                      new A.GraphicData(
                      new PIC.Picture(
                      new PIC.NonVisualPictureProperties(
                      new PIC.NonVisualDrawingProperties()
                      {
                          Id = (UInt32Value)0U,
                          Name = "New Bitmap Image.jpg"
                      },
                      new PIC.NonVisualPictureDrawingProperties()),
                      new PIC.BlipFill(
                      new A.Blip(
                      new A.BlipExtensionList(
                      new A.BlipExtension()
                      {
                          Uri =
                      "{28A0092B-C50C-407E-A947-70E740481C1C}"
                      })
                      )
                      {
                          Embed = relationshipId,
                          CompressionState =
                      A.BlipCompressionValues.Print
                      },
                      new A.Stretch(
                      new A.FillRectangle())),
                      new PIC.ShapeProperties(
                      new A.Transform2D(
                      new A.Offset() { X = 0L, Y = 0L },
                      new A.Extents() { Cx = 5248275L, Cy = 2895600L }), //与上面的对准
                                           new A.PresetGeometry(
                      new A.AdjustValueList()
                      )
                                           { Preset = A.ShapeTypeValues.Rectangle }))
                      )
                      { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                      )
                      {
                          DistanceFromTop = (UInt32Value)0U,
                          DistanceFromBottom = (UInt32Value)0U,
                          DistanceFromLeft = (UInt32Value)0U,
                          DistanceFromRight = (UInt32Value)0U,
                          EditId = "50D07946"
                      });

            // Append the reference to body, the element should be in a Run.
            wordDocp.AppendChild(new Paragraph(new Run(element)));
        }
    }
}
