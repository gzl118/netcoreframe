using System;
using System.Collections.Generic;

namespace SANS.DbEntity.Models
{
    public partial class SysUserGroup
    {
        public string UserGroupId { get; set; }
        public string UserGroupName { get; set; }
        public string ParentUserGroupId { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? EditTime { get; set; }
        public int DeleteSign { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string Note { get; set; }
    }
}
