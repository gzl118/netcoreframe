using System;
using System.Collections.Generic;
using System.Text;
using SANS.DbEntity.Models;

namespace SANS.BLL.Interface
{
    /// <summary>
    /// 权限表Service
    /// </summary>
    public interface ISysAuthorityBLL : IBaseBLL<SysAuthority>
    {
        /// <summary>
        /// 获取用户的权限集合
        /// </summary>
        /// <param name="user">当前登陆用户</param>
        /// <param name="authorityType">权限类型</param>
        /// <returns></returns>
        MessageModel GetSysAuthorities(SysUser user, SysEnum.Enum_AuthorityType authorityType);
        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="RoleId">角色Id</param>
        /// <param name="authorityType">权限类型</param>
        /// <returns></returns>
        MessageModel GetRoleAuthoritieList(string RoleId, SysEnum.Enum_AuthorityType authorityType);
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        List<string> GetSysBtn(SysUser user, string oid);
    }
}
