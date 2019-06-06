using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using A = DocumentFormat.OpenXml.Drawing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using OpenXmlPowerTools;
using Table = DocumentFormat.OpenXml.Wordprocessing.Table;
using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;

namespace SANS.Common.OpenXml
{
    public class WordOperate
    {
        public static bool IsPaperOrientation { get; private set; }

        public void CreateOpenXMLFile(string filePath, bool IsAddHead = false)
        {
            using (WordprocessingDocument objWordDocument = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart objMainDocumentPart = objWordDocument.AddMainDocumentPart();
                objMainDocumentPart.Document = new Document(new Body());
                Body objBody = objMainDocumentPart.Document.Body;
                //创建一些需要用到的样式,如标题3,标题4,在OpenXml里面,这些样式都要自己来创建的 
                //ReportExport.CreateParagraphStyle(objWordDocument);
                SectionProperties sectionProperties = new SectionProperties();
                PageSize pageSize = new PageSize();
                PageMargin pageMargin = new PageMargin();
                Columns columns = new Columns() { Space = "220" };//720
                DocGrid docGrid = new DocGrid() { LinePitch = 100 };//360
                //创建页面的大小,页距,页面方向一些基本的设置,如A4,B4,Letter, 
                //GetPageSetting(PageSize,PageMargin);

                //在这里填充各个Paragraph,与Table,页面上第一级元素就是段落,表格.
                objBody.Append(new Paragraph());
                ReportTable rt = new ReportTable();
                rt.Column = 3;
                rt.Value = new List<List<string>>();
                rt.Value.Add(new List<string>() { "1", "2", "3" });
                rt.Value.Add(new List<string>() { "2", "3", "4" });
                rt.Value.Add(new List<string>() { "3", "4", "5" });
                rt.Value.Add(new List<string>() { "4", "5", "6" });
                objBody.Append(GetParagraph(rt, 600));
                objBody.Append(new Paragraph());

                //我会告诉你这里的顺序很重要吗?下面才是把上面那些设置放到Word里去.(大家可以试试把这下面的代码放上面,会不会出现打开openxml文件有误,因为内容有误)
                sectionProperties.Append(pageSize, pageMargin, columns, docGrid);
                objBody.Append(sectionProperties);

                //如果有页眉,在这里添加页眉.
                if (IsAddHead)
                {
                    //添加页面,如果有图片,这个图片和上面添加在objBody方式有点不一样,这里搞了好久.
                    //ReportExport.AddHeader(objMainDocumentPart, image);
                }
                objMainDocumentPart.Document.Save();
            }
        }
        // 为文档创建段落样式
        public static void CreateParagraphStyle(WordprocessingDocument doc)
        {
            // 进入文档控制样式部分
            StyleDefinitionsPart styleDefinitionsPart;
            styleDefinitionsPart = doc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            Styles root = new Styles();
            root.Save(styleDefinitionsPart);

            Styles styles = styleDefinitionsPart.Styles;
            if (styles == null)
            {
                styleDefinitionsPart.Styles = new Styles();
                styleDefinitionsPart.Styles.Save();
            }

            Style style3 = CreateTitleStyle(3);
            Style style4 = CreateTitleStyle(4);
            // 把样式添加入文档中
            styles.Append(style3);
            styles.Append(style4);
        }

        private static Style CreateTitleStyle(int titleIndex)
        {
            string titleID = titleIndex.ToString();
            string rsid = string.Empty;
            string before = string.Empty;
            string after = string.Empty;
            string line = string.Empty;
            string val = string.Empty;
            int outline = titleIndex - 1;
            if (titleIndex == 3)
            {
                rsid = "00BA1E98";
                before = "130";//"260"
                after = "0";
                line = "286";//"416"
                val = "32";

            }
            else if (titleIndex == 4)
            {
                rsid = "00BA1E98";
                before = "88";
                after = "0";
                line = "288";//"376"
                val = "28";
            }

            Style style2 = new Style() { Type = StyleValues.Paragraph, StyleId = titleID };
            StyleName styleName2 = new StyleName() { Val = "heading " + titleID };
            BasedOn basedOn1 = new BasedOn() { Val = "a" };
            NextParagraphStyle nextParagraphStyle1 = new NextParagraphStyle() { Val = "a" };
            LinkedStyle linkedStyle1 = new LinkedStyle() { Val = titleID + "Char" };
            UIPriority uIPriority1 = new UIPriority() { Val = 9 };
            PrimaryStyle primaryStyle2 = new PrimaryStyle();
            Rsid rsid2 = new Rsid() { Val = rsid };
            style2.Append(styleName2);
            style2.Append(basedOn1);
            style2.Append(nextParagraphStyle1);
            style2.Append(linkedStyle1);
            style2.Append(uIPriority1);
            style2.Append(primaryStyle2);
            style2.Append(rsid2);

            StyleParagraphProperties styleParagraphProperties2 = new StyleParagraphProperties();
            KeepNext keepNext1 = new KeepNext();
            KeepLines keepLines1 = new KeepLines();
            SpacingBetweenLines spacingBetweenLines1 = new SpacingBetweenLines() { Before = before, After = after, Line = line, LineRule = LineSpacingRuleValues.Auto };
            OutlineLevel outlineLevel1 = new OutlineLevel() { Val = outline };
            styleParagraphProperties2.Append(keepNext1);
            styleParagraphProperties2.Append(keepLines1);
            styleParagraphProperties2.Append(spacingBetweenLines1);
            styleParagraphProperties2.Append(outlineLevel1);
            style2.Append(styleParagraphProperties2);

            StyleRunProperties styleRunProperties1 = new StyleRunProperties();
            Bold bold1 = new Bold();
            BoldComplexScript boldComplexScript1 = new BoldComplexScript();
            // Kern kern2 = new Kern() { Val = (UInt32)44U };
            FontSize fontSize2 = new FontSize() { Val = val };
            FontSizeComplexScript fontSizeComplexScript2 = new FontSizeComplexScript() { Val = val };
            styleRunProperties1.Append(bold1);
            styleRunProperties1.Append(boldComplexScript1);
            //styleRunProperties1.Append(kern2);
            styleRunProperties1.Append(fontSize2);
            styleRunProperties1.Append(fontSizeComplexScript2);
            style2.Append(styleRunProperties1);
            return style2;
        }
        public static void GetPageSetting(ref PageSize pageSize, ref PageMargin pageMargin)
        {
            bool val = IsPaperOrientation;
            string str_paperSize = "Letter";//A4,B4
            UInt32Value width = 15840U;
            UInt32Value height = 12240U;
            int top = 1440;
            UInt32Value left = 1440U;
            if (str_paperSize == "A4")
            {
                width = 16840U;
                height = 11905U;
            }
            else if (str_paperSize == "B4")
            {
                width = 20636U;
                height = 14570U;
            }

            if (!val)
            {
                UInt32Value sweep = width;
                width = height;
                height = sweep;

                int top_sweep = top;
                top = (int)left.Value;
                left = (uint)top_sweep;
            }

            pageSize.Width = width;
            pageSize.Height = height;
            pageSize.Orient = new EnumValue<PageOrientationValues>(val ? PageOrientationValues.Landscape : PageOrientationValues.Portrait);

            pageMargin.Top = top;
            pageMargin.Bottom = top;
            pageMargin.Left = left;
            pageMargin.Right = left;
            pageMargin.Header = (UInt32Value)720U;
            pageMargin.Footer = (UInt32Value)720U;
            pageMargin.Gutter = (UInt32Value)0U;
        }
        private static List<Run> GetRuns(ReportCommon common)
        {
            List<Run> runs = new List<Run>();
            if (common is ReportValue)
            {
                ReportValue reportvalue = common as ReportValue;
                Run r = new Run();
                RunProperties rP = GetRunProperties(reportvalue, true);
                r.Append(rP);
                string text = reportvalue.Text;
                if (text.EndsWith(":"))
                    text = text + " ";
                if (!text.EndsWith(": "))
                    text = text + ": ";
                Text t = CreateText(text);
                r.Append(t);
                runs.Add(r);

                r = new Run();
                rP = GetRunProperties(reportvalue, false);
                r.Append(rP);
                r.Append(CreateText(reportvalue.Value));
                runs.Add(r);
            }
            else if (common is ReportImage)
            {
                ReportImage reportImage = common as ReportImage;
                Run r = new Run();
                RunProperties rP = GetRunProperties(reportImage);
                Drawing image = GetImageToBody(reportImage.RId, reportImage.Width * 600, reportImage.Height * 800);
                //Drawing image = new Drawing();
                //image.Append(new A.Blip() { Embed = new StringValue(reportImage.RId) });
                r.Append(rP);
                r.Append(image);
                runs.Add(r);
            }
            else if (common is ReportText)
            {
                Run r = new Run();
                RunProperties rP = GetRunProperties(common);
                r.Append(rP);
                r.Append(CreateText(common.Text));
                runs.Add(r);
            }
            return runs;
        }
        public static RunProperties GetRunProperties(ReportCommon common, bool bBold = false)
        {
            RunProperties rPr = new RunProperties();
            //Color color = new Color() { Val = "FF0000" }; // the color is red
            RunFonts rFont = new RunFonts();
            rFont.Ascii = "Arial"; // the font is Arial
            //rPr.Append(color);
            //rPr.Append(rFont);
            if (common.IsBold || bBold)
                rPr.Append(new Bold()); // it is Bold
            //TextAlignment 
            rPr.Append(new FontSize() { Val = new StringValue((common.Size * 2).ToString()) }); //font size (in 1/72 of an inch)
            return rPr;
        }
        private static Text CreateText(string text)
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
        private static Drawing GetImageToBody(string relationshipId, int x = 914400, int y = 360000)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = x, Cy = y },
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
                                             new A.Extents() { Cx = x, Cy = y }),
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
            var blip = element.Descendants<DocumentFormat.OpenXml.Drawing.Blip>()
               .FirstOrDefault<DocumentFormat.OpenXml.Drawing.Blip>();
            return element;
        }
        public static void CreateImageRid(ReportImage reportImage, MainDocumentPart objMainDocumentPart)
        {
            ImagePartType imagetype = ImagePartType.Jpeg;
            FileInfo newImg = new FileInfo(reportImage.Value);
            ImagePart newImgPart = objMainDocumentPart.AddImagePart(imagetype);
            //插入图片数据到Word里去.
            using (FileStream stream = newImg.OpenRead())
            {
                newImgPart.FeedData(stream);
            }
            //Word返回给我们插入数据的标识符.
            reportImage.RId = objMainDocumentPart.GetIdOfPart(newImgPart);
        }
        public static Table GetParagraph(ReportTable reportTable, Int32Value width)
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

        public static void SetTableParagraph(ReportTable reportTable, Table table)
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
        public static void AddHeader(MainDocumentPart mainDocPart, ReportImage reportImge)
        {
            // Delete the existing header parts.
            mainDocPart.DeleteParts(mainDocPart.HeaderParts);

            // Create a new header part and get its relationship id.
            HeaderPart newHeaderPart = mainDocPart.AddNewPart<HeaderPart>();
            string rId = mainDocPart.GetIdOfPart(newHeaderPart);

            ImagePart imagepart = newHeaderPart.AddImagePart(ImagePartType.Jpeg);
            FileInfo newImg = new FileInfo(reportImge.Value);
            using (FileStream stream = newImg.OpenRead())
            {
                imagepart.FeedData(stream);
            }
            string imageRID = newHeaderPart.GetIdOfPart(imagepart);
            reportImge.RId = imageRID;
            Header header = GeneratePageHeaderPart(reportImge);
            header.Save(newHeaderPart);

            foreach (SectionProperties sectProperties in
              mainDocPart.Document.Descendants<SectionProperties>())
            {
                //  Delete any existing references to headers.
                foreach (HeaderReference headerReference in
                  sectProperties.Descendants<HeaderReference>())
                    sectProperties.RemoveChild(headerReference);

                HeaderReference newHeaderReference =
                  new HeaderReference() { Id = rId, Type = HeaderFooterValues.Default };
                sectProperties.Append(newHeaderReference);
            }
            header.Save();
        }
        public static List<Paragraph> GetParagraph(ReportValueList valueList, Int32Value width, int column = 2)
        {
            if (column < 1)
                column = 1;
            List<Paragraph> list = new List<Paragraph>();
            int currentcolumn = 0;
            Paragraph currentParagraph = null;
            foreach (var reportvalue in valueList.Values)
            {
                reportvalue.Size = valueList.Size;
                if (currentcolumn == 0)
                {
                    currentParagraph = new Paragraph();
                    ParagraphProperties pPr = new ParagraphProperties();
                    //添加标签类
                    Tabs tabs = new Tabs();
                    Int32Value eachWidth = width / (new Int32Value(column));
                    for (int i = 1; i < column; i++)
                    {
                        TabStop stop = new TabStop();
                        stop.Val = new EnumValue<TabStopValues>(TabStopValues.Left);
                        stop.Position = eachWidth * i;
                        tabs.Append(stop);
                    }
                    pPr.Append(tabs);
                    currentParagraph.Append(pPr);
                    list.Add(currentParagraph);
                }
                List<Run> runs = GetRuns(reportvalue);
                foreach (var run in runs)
                {
                    currentParagraph.Append(run);
                }
                currentcolumn++;
                if (currentcolumn < column)
                {
                    Run run = new Run();
                    run.Append(new TabChar());
                    currentParagraph.Append(run);
                }
                if (currentcolumn >= column)
                {
                    currentcolumn = 0;
                }
            }
            return list;
        }
        // Creates an header instance and adds its children.  
        private static Header GeneratePageHeaderPart(ReportImage reportImge)
        {
            var runs = GetRuns(reportImge);
            Paragraph paragraph = new Paragraph();
            paragraph.Append(GetParagraphProperties(reportImge));
            foreach (var run in runs)
            {
                paragraph.Append(run);
            }
            paragraph.Append(new Run(new Text() { Text = "" }));
            Header header = new Header();
            header.Append(paragraph);
            return header;
        }

        private static IEnumerable<OpenXmlElement> GetParagraphProperties(ReportImage reportImge)
        {
            throw new NotImplementedException();
        }

        public static void WordTextReplacer(string filePath, string key, string value)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                TextReplacer.SearchAndReplace(doc, key, value, false);
            }
        }
        public static void WordTextReplacer(string filePath, string key, string value,bool b)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                TextReplacer.SearchAndReplace(doc, key, value, b);
            }
        }
        public static void AddImageToBody(Paragraph wordDocp, string relationshipId)
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

        public static void ReadWordByOpenXml(string path, string SearchString)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(path, true))
            {
                Body body = doc.MainDocumentPart.Document.Body;

                foreach (var inst in body.Elements<OpenXmlElement>())
                {
                    if (inst.InnerText.IndexOf(SearchString) > 0)
                    {
                        var t = inst.InnerText;
                        //inst.Parent.
                    }
                }
                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    var t = paragraph.InnerText;
                    if (t.IndexOf("«TableCycle") >= 0)
                    {
                        var paragraphText = t.Replace("«TableCycle", "");
                        ReportTable rt = new ReportTable();
                        rt.IsHaveColumn = true;
                        rt.Column = 3;
                        rt.Value = new List<List<string>>();
                        rt.Value.Add(new List<string>() { "事件名称", "起始时间", "终止时间" });
                        rt.Value.Add(new List<string>() { "2", "3", "4" });
                        rt.Value.Add(new List<string>() { "3", "4", "5" });
                        rt.Value.Add(new List<string>() { "4", "5", "6" });

                        paragraph.Append(GetParagraph(rt, 1000));
                    }
                    if (t.IndexOf("«Chart") >= 0)
                    {
                        var picturePath = "doc/ww.png";
                        string picType = picturePath.Split('.').Last();
                        ImagePartType imagePartType;
                        ImagePart imagePart = null;
                        // 通过后缀名判断图片类型, true 表示忽视大小写
                        if (Enum.TryParse<ImagePartType>(picType, true, out imagePartType))
                        {
                            imagePart = doc.MainDocumentPart.AddImagePart(imagePartType);
                        }
                        try
                        {
                            imagePart.FeedData(File.Open(picturePath, FileMode.Open));
                        }
                        catch (Exception er)
                        {

                        }
                        var relationshipId = doc.MainDocumentPart.GetIdOfPart(imagePart);
                        AddImageToBody(paragraph, relationshipId);
                    }
                }

                foreach (var table in body.Elements<Table>())
                {
                    foreach (var tableRow in table.Elements<TableRow>())
                    {
                        var ttr = false;
                        foreach (var tableCell in tableRow.Elements<TableCell>())
                        {
                            var t = tableCell.InnerText;
                            if (t.IndexOf("«TableEvent") >= 0)
                            {
                                ttr = true;
                                break;
                            }
                        }
                        if (ttr)
                        {
                            table.RemoveChild(tableRow);
                            ReportTable rt = new ReportTable();
                            rt.IsHaveColumn = false;
                            rt.Column = 3;
                            rt.Value = new List<List<string>>();
                            rt.Value.Add(new List<string>() { "1", "2", "3" });
                            rt.Value.Add(new List<string>() { "2", "3", "4" });
                            rt.Value.Add(new List<string>() { "3", "4", "5" });
                            rt.Value.Add(new List<string>() { "4", "5", "6" });
                            SetTableParagraph(rt, table);
                        }
                    }
                }
                foreach (var table in body.Elements<Table>())
                {
                    foreach (var tableRow in table.Elements<TableRow>())
                    {
                        Console.Write(tableRow.InnerText);
                    }
                }
                foreach (var table in body.Elements<Table>())
                {
                    Console.Write(table.InnerText);
                }
            }
        }
    }
    
}
