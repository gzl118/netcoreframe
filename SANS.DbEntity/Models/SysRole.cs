using System;
using System.Collections.Generic;

namespace SANS.DbEntity.Models
{
    public partial class SysRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int DeleteSign { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string Note { get; set; }
    }
}
