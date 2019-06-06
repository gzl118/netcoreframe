using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Common.OpenXml
{
    public class ReportCommon
    {
        public const float defaultSize = 12f;
        public const float H1 = 20f;
        public const float H3 = 16f;
        public const float H4 = 14f;
        private string text = string.Empty;
        public ReportCommon()
        {
            Alignment = -1;
            Size = defaultSize;
        }
        //规定-1左对齐,0中间,1右对齐
        public int Alignment { get; set; }

        public virtual float Size { get; set; }

        public string Text { get; set; }

        public virtual bool IsBold { get; set; }
    }
    public class ReportValueList
    {
        public IEnumerable<ReportValue> Values { get;  set; }
        public float Size { get;  set; }
    }

    public class ReportValue : ReportCommon
    {
        public string Value { get;  set; }
        public string Key { get;  set; }
    }
    public class ReportImage : ReportCommon
    {
        public string Value { get;  set; }
        public string RId { get;  set; }
        public int Width { get;  set; }
        public int Height { get;  set; }
    }
    public class ReportText : ReportCommon
    {

    }
    public class ReportTable : ReportCommon
    {
        public int Column { get;  set; }
        public bool IsHaveColumn { get;  set; }
        public List<List<string>> Value { get;  set; }
    }
}
