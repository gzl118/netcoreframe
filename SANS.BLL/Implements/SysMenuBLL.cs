using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;
using SANS.BLL.Interface;
using SANS.DAL.Interface;
using SANS.DbEntity.Models;

namespace SANS.BLL.Implements
{
    /// <summary>
    /// 菜单表Service接口
    /// </summary>
    public class SysMenuBLL : BaseBLL<SysMenu>, ISysMenuBLL
    {
        private List<SysMenuDto> listMenuDtos;
        private readonly ISysMenuDAL sysMenuDAL;
        private readonly ISysAuthorityBLL sysAuthorityBLL;
        private readonly ISysAmRelatedDAL sysAmRelatedDAL;
        /// <summary>
        /// 用于实例化父级，sysMenuDAL
        /// </summary>
        /// <param name="sysMenuDAL"></param>
        public SysMenuBLL(ISysMenuDAL sysMenuDAL, ISysAuthorityBLL sysAuthorityBLL, ISysAmRelatedDAL sysAmRelatedDAL) : base(sysMenuDAL)
        {
            this.sysMenuDAL = sysMenuDAL;
            this.sysAuthorityBLL = sysAuthorityBLL;
            this.sysAmRelatedDAL = sysAmRelatedDAL;
        }
        /// <summary>
        /// 根据用户获取功能菜单
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public MessageModel GetMenusBy(SysUser user, SysEnum.Enum_AuthorityType mType)
        {
            var listMenuDto = new List<SysMenuDto>();
            listMenuDtos = Mapper.Map<List<SysMenuDto>>(((List<SysAuthority>)(sysAuthorityBLL.GetSysAuthorities(user, mType).Data)).Select(t => t.sysMenu).ToList());
            //找出所有一级菜单
            listMenuDto.AddRange(listMenuDtos.Where(t => t.ParentMenuId.Equals(Guid.Empty.ToString())).OrderBy(t => t.MenuSort).ThenBy(t => t.CreateTime));
            foreach (var item in listMenuDto)
            {
                item.children = GetMenuChildren(item.MenuId);
            }
            return new MessageModel
            {
                Data = listMenuDto
            };
        }
        /// <summary>
        /// 获取子集菜单
        /// </summary>
        /// <param name="ParentMenuId"></param>
        /// <returns></returns>
        private List<SysMenuDto> GetMenuChildren(string ParentMenuId)
        {
            var listMenuDto = listMenuDtos.Where(t => t.ParentMenuId.Equals(ParentMenuId)).OrderBy(t => t.MenuSort).ThenBy(t => t.CreateTime).ToList();
            foreach (var item in listMenuDto)
            {
                item.children = GetMenuChildren(item.MenuId);
            }

            return listMenuDto;
        }
        /// <summary>
        /// 删除菜单(逻辑删除)
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        public MessageModel DelMenu(string MenuId)
        {
            bool bResult = false;
            //获取菜单
            var menuModel = sysMenuDAL.GetModels(t => t.MenuId.Equals(MenuId)).First();
            //获取菜单与权限关联表
            var relatedModel = sysAmRelatedDAL.GetModels(t => t.MenuId.Equals(MenuId)).First();
            var authorityModel = sysAuthorityBLL.GetModels(y => y.AuthorityId.Equals(relatedModel.AuthorityId)).First();
            using (var tran = new TransactionScope())
            {
                int i = 0;
                menuModel.DeleteSign = (int)SysEnum.Enum_DeleteSign.Sign_Undeleted;
                menuModel.DeleteTime = DateTime.Now;
                sysMenuDAL.Update(menuModel);
                if (sysMenuDAL.SaveChanges()) i++;
                authorityModel.DeleteSign = (int)SysEnum.Enum_DeleteSign.Sign_Undeleted;
                authorityModel.DeleteTime = DateTime.Now;
                if (sysAuthorityBLL.Update(authorityModel)) i++;

                if (i == 2)
                {
                    bResult = true;
                    tran.Complete();
                }
                else
                {
                    bResult = false;
                    tran.Dispose();
                }
            }
            return new MessageModel
            {
                Message = "删除失败",
                Result = bResult
            };
        }
        /// <summary>
        /// 获取角色的菜单
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public MessageModel GetRoleMenuList(string RoleId)
        {

            var list = Mapper.Map<List<SysMenuDto>>(((List<SysAuthority>)sysAuthorityBLL.GetRoleAuthoritieList(RoleId, SysEnum.Enum_AuthorityType.ALL).Data).Select(t => t.sysMenu).ToList());
            return new MessageModel
            {
                Data = list
            };
        }
        public List<SysMenu> GetMenuBtns(SysUser user, string oid)
        {
            List<SysRole> roles = new List<SysRole>(user.sysRoles);
            if (user.sysUserGroup != null)
                roles.AddRange(user.sysUserGroup.sysRoles);
            return sysAuthorityBLL.GetSysBtn(user, oid);
        }
    }
}
