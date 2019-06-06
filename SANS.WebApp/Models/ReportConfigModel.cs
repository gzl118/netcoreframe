using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Models
{
    public class ReportConfigModel
    {
        public string variableName{get;set;}
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string TimeSpan { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
