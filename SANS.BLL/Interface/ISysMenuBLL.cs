using System;
using System.Collections.Generic;
using System.Text;
using SANS.DbEntity.Models;

namespace SANS.BLL.Interface
{
    /// <summary>
    /// 菜单表Service接口
    /// </summary>
    public interface ISysMenuBLL : IBaseBLL<SysMenu>
    {
        /// <summary>
        /// 根据用户获取功能菜单
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        MessageModel GetMenusBy(SysUser user, SysEnum.Enum_AuthorityType mType);
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        MessageModel DelMenu(string MenuId);
        /// <summary>
        /// 获取角色的权限菜单
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        MessageModel GetRoleMenuList(string RoleId);
        /// <summary>
        /// 获取菜单按钮权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oid"></param>
        /// <returns></returns>
        List<string> GetMenuBtns(SysUser user, string oid);
    }
}
