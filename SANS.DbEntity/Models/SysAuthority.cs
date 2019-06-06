using System;
using System.Collections.Generic;

namespace SANS.DbEntity.Models
{
    public partial class SysAuthority
    {
        public string AuthorityId { get; set; }
        public int AuthorityType { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int DeleteSign { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string Note { get; set; }
    }
}
