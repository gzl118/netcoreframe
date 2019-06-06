using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.DbEntity.Models
{
    public partial class SysDataDictionary
    {
       public string dict_id { get; set; }
        public string type_code{ get;set;}
        public string type_name{ get;set;}
        public string code { get; set; }
        public string name { get; set; }
        public string code_value { get; set; }
        public int code_sort { get; set; }
        public DateTime? creat_time { get; set; }
        public string parent_code { get; set; }
        public string remark { get; set; }
    }
}
