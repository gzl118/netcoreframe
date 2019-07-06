#region 版权声明
/**************************************************************** 
 * 作    者：周黎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/7/8 16:02:42 
 * 当前版本：1.0.0.1 
 *  
 * 描述说明： 
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ ZhouLi 2018 All rights reserved 
 *┌──────────────────────────────────┐
 *│　此技术信息周黎的机密信息，未经本人书面同意禁止向第三方披露．　│
 *│　版权所有：周黎 　　　　　　　　　　　　　　│
 *└──────────────────────────────────┘
*****************************************************************/
#endregion
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using SANS.DbEntity.Models;
using SANS.Common;

namespace SANS.WebApp.Data
{
    /// <summary>
    /// 初始化系统数据库
    /// </summary>
    public class InitSystem
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool InitDB(IServiceProvider service)
        {
            using (var serviceScope = service.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MyEFContext>();
                var userAdmin = context.SysUser.Where(t => t.UserName.Equals("admin")).SingleOrDefault();
                if (userAdmin == null)
                {
                    using (var tran = context.Database.BeginTransaction())
                    {
                        try
                        {
                            #region 初始化权限管理数据
                            #region 添加用户
                            //初始化用户(添加超级管理员:zhouli)
                            var entityUser = context.SysUser.Add(new SysUser
                            {
                                UserName = "admin",
                                UserNikeName = "admin",
                                UserPwd = MD5Encrypt.Get32MD5One("123456"),
                                Note = "系统初始化自动添加的"
                            });

                            //这里不对超级管理员做任何授权,因为超级管理没有任何限制
                            ////添加角色
                            //var entityRole = context.SysRole.Add(new SysRole
                            //{
                            //    RoleName = "超级管理员",
                            //    Note = "系统初始化自动添加的"
                            //});
                            ////添加用户角色关联表
                            //context.SysUrRelated.Add(new SysUrRelated
                            //{
                            //    UserId = entityUser.Entity.UserId,
                            //    RoleId = entityRole.Entity.RoleId
                            //});
                            ////添加用户组
                            //var entityUserGroup = context.SysUserGroup.Add(new SysUserGroup
                            //{
                            //    UserGroupName = "超级管理员组",
                            //    Note = "系统初始化自动添加的"

                            //});
                            ////添加用户组与用户关联表
                            //context.SysUuRelated.Add(new SysUuRelated
                            //{
                            //    UserId = entityUser.Entity.UserId,
                            //    UserGroupId = entityUserGroup.Entity.UserGroupId
                            //});
                            ////添加角色与用户组关联表
                            //context.SysUgrRelated.Add(new SysUgrRelated
                            //{
                            //    UserGroupId = entityUserGroup.Entity.UserGroupId,
                            //    RoleId = entityRole.Entity.RoleId
                            //});
                            //添加菜单,权限,权限菜单关联表(因为权限表与权限菜单关联表、权限菜单关联表与菜单表都是一对一的关系，也就是每添加一个菜单，就得同时往这三个表中各插入一条记录)
                            #endregion
                            #region 添加系统菜单
                            //----------------------------系统管理菜单begin
                            var entityMenu = context.SysMenu.Add(new SysMenu
                            {
                                MenuName = "系统管理",
                                MenuSort = 1,
                                MenuId = Guid.NewGuid().ToString()

                            });
                            var entityAuthority = context.SysAuthority.Add(new SysAuthority
                            {
                                AuthorityType = 1,
                                AuthorityId = Guid.NewGuid().ToString()
                            });
                            context.SysAmRelated.Add(new SysAmRelated
                            {
                                MenuId = entityMenu.Entity.MenuId,
                                AuthorityId = entityAuthority.Entity.AuthorityId
                            });
                            //----------------------------系统管理菜单end
                            //----------------------------权限管理菜单begin
                            var entityMenu1 = context.SysMenu.Add(new SysMenu
                            {
                                MenuName = "角色授权",
                                MenuUrl = "/System/Authority/Index",
                                ParentMenuId = entityMenu.Entity.MenuId,
                                MenuId = Guid.NewGuid().ToString()

                            });
                            var entityAuthority1 = context.SysAuthority.Add(new SysAuthority
                            {
                                AuthorityType = 1,
                                AuthorityId = Guid.NewGuid().ToString()
                            });
                            context.SysAmRelated.Add(new SysAmRelated
                            {
                                MenuId = entityMenu1.Entity.MenuId,
                                AuthorityId = entityAuthority1.Entity.AuthorityId
                            });
                            //----------------------------权限管理菜单end
                            //----------------------------菜单管理菜单begin
                            var entityMenu2 = context.SysMenu.Add(new SysMenu
                            {
                                MenuName = "菜单管理",
                                MenuUrl = "/System/Menu/Index",
                                ParentMenuId = entityMenu.Entity.MenuId,
                                MenuId = Guid.NewGuid().ToString()
                            });
                            var entityAuthority2 = context.SysAuthority.Add(new SysAuthority
                            {
                                AuthorityType = 1,
                                AuthorityId = Guid.NewGuid().ToString()

                            });
                            context.SysAmRelated.Add(new SysAmRelated
                            {
                                MenuId = entityMenu2.Entity.MenuId,
                                AuthorityId = entityAuthority2.Entity.AuthorityId
                            });
                            //----------------------------菜单管理菜单end
                            //----------------------------角色管理菜单begin
                            var entityMenu3 = context.SysMenu.Add(new SysMenu
                            {
                                MenuName = "角色管理",
                                MenuUrl = "/System/Role/Index",
                                ParentMenuId = entityMenu.Entity.MenuId,
                                MenuId = Guid.NewGuid().ToString()
                            });
                            var entityAuthority3 = context.SysAuthority.Add(new SysAuthority
                            {
                                AuthorityType = 1,
                                AuthorityId = Guid.NewGuid().ToString()
                            });
                            context.SysAmRelated.Add(new SysAmRelated
                            {
                                MenuId = entityMenu3.Entity.MenuId,
                                AuthorityId = entityAuthority3.Entity.AuthorityId
                            });
                            //----------------------------角色管理菜单end
                            //----------------------------用户管理菜单begin
                            var entityMenu4 = context.SysMenu.Add(new SysMenu
                            {
                                MenuName = "用户管理",
                                MenuUrl = "/System/User/Index",
                                ParentMenuId = entityMenu.Entity.MenuId,
                                MenuId = Guid.NewGuid().ToString()
                            });
                            var entityAuthority4 = context.SysAuthority.Add(new SysAuthority
                            {
                                AuthorityType = 1,
                                AuthorityId = Guid.NewGuid().ToString()
                            });
                            context.SysAmRelated.Add(new SysAmRelated
                            {
                                MenuId = entityMenu4.Entity.MenuId,
                                AuthorityId = entityAuthority4.Entity.AuthorityId
                            });
                            //----------------------------用户管理菜单end
                            //----------------------------用户组菜单begin
                            var entityMenu5 = context.SysMenu.Add(new SysMenu
                            {
                                MenuName = "用户组管理",
                                MenuUrl = "/System/UserGroup/Index",
                                ParentMenuId = entityMenu.Entity.MenuId,
                                MenuId = Guid.NewGuid().ToString()
                            });
                            var entityAuthority5 = context.SysAuthority.Add(new SysAuthority
                            {
                                AuthorityType = 1,
                                AuthorityId = Guid.NewGuid().ToString()
                            });
                            context.SysAmRelated.Add(new SysAmRelated
                            {
                                MenuId = entityMenu5.Entity.MenuId,
                                AuthorityId = entityAuthority5.Entity.AuthorityId
                            });
                            //----------------------------用户组菜单end
                            #endregion
                            #endregion
                            context.SaveChanges();
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
            return true;
        }
    }
}
