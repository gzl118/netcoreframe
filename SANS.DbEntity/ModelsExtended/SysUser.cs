using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SANS.DbEntity.Models
{
    public partial class SysUser
    {
        [NotMapped]
        /// <summary>
        /// 所属用户组
        /// </summary>
        public SysUserGroup SysUserGroup { set; get; }
        [NotMapped]
        /// <summary>
        /// 用户所拥有的角色
        /// </summary>
        public List<SysRole> SysRoles { set; get; }
        [NotMapped]
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsAdministrctor { set; get; }
        [NotMapped]
        /// <summary>
        /// 是否是业务超级管理员
        /// </summary>
        public bool IsBusinessAdministrctor { set; get; }
    }
}
