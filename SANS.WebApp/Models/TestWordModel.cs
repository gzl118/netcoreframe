using SANS.Common.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Models
{
    public class TestWordModel
    {
        [PlaceHolder(PlaceHolderEnum.A)]
        public string ReportName { get; set; }

        [PlaceHolder(PlaceHolderEnum.B)]
        public string AlarmData { get; set; }

        [PicturePlaceHolder(PlaceHolderEnum.C, "报警图片")]
        public List<string> AlarmImages { get; set; }
        [TablePlaceHolder(PlaceHolderEnum.D)]
        public DataTable AlarmTable { get; set; }

    }
    public class TestWordModelCS
    {
        [PlaceHolder(PlaceHolderEnum.A)]
        public string ReportInfo { get; set; }
        [TablePlaceHolder(PlaceHolderEnum.B)]
        public DataTable CycleTable { get; set; }
        [PlaceHolder(PlaceHolderEnum.C)]
        public string TParameter { get; set; }
        [PicturePlaceHolder(PlaceHolderEnum.D, "图片")]
        public List<string> ParameterImages { get; set; }


    }
}
