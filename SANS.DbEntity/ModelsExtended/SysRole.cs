using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SANS.DbEntity.Models
{
    public partial class SysRole
    {
        [NotMapped]
        /// <summary>
        /// 角色对应的权限集合
        /// </summary>
        public List<SysAuthority> sysAuthorities { set; get; }
    }
}
