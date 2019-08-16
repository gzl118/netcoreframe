using Dapper;
using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SANS.DAL.Implements
{
    public class SysAuthorityDAL : BaseDAL<SysAuthority>, ISysAuthorityDAL
    {

        public SysAuthorityDAL(DapperContext dapper, MyEFContext db) : base(dapper, db)
        {
        }

        public bool Delete(List<string> list)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取角色的权限集合
        /// </summary>
        /// <param name="isAdmin">是否超级管理员</param>
        /// <param name="roles">角色集合</param>
        /// <param name="authorityType">权限类型</param>
        /// <returns></returns>
        public List<SysAuthority> GetSysAuthorities(Boolean isAdmin, List<SysRole> roles, SysEnum.Enum_AuthorityType authorityType)
        {
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine($@"SELECT DISTINCT SAT.*, SM.*
                                FROM(
                                    SELECT SAT.*
                                    FROM Sys_Authority SAT WHERE DeleteSign={(int)SysEnum.Enum_DeleteSign.Sing_Deleted} ) SAT");
            if (!isAdmin)
            {
                var roleList = roles.Select(t => t.RoleId).ToList();
                if (roleList.Count() == 0)
                    roleList.Add(Guid.Empty.ToString());
                builder.AppendLine($@"INNER JOIN (
                                SELECT srar.AuthorityId
                                FROM Sys_Role srol, Sys_RaRelated srar
                                WHERE srol.RoleId = srar.RoleId
                                    AND srol.RoleId IN ('{string.Join("','", roleList)}')
                            ) t
                            ON t.AuthorityId = SAT.AuthorityId");
            }
            switch (authorityType)
            {
                case SysEnum.Enum_AuthorityType.Type_Menu:
                    builder.AppendLine($@"
                                INNER JOIN Sys_AmRelated SAR ON SAR.AuthorityId = SAT.AuthorityId
                                INNER JOIN Sys_Menu SM ON SAR.MenuId = SM.MenuId AND (SM.MenuType=0 OR SM.MenuType=1)");
                    break;
                case SysEnum.Enum_AuthorityType.Type_Button:
                    builder.AppendLine($@"
                                INNER JOIN Sys_AmRelated SAR ON SAR.AuthorityId = SAT.AuthorityId
                                INNER JOIN Sys_Menu SM ON SAR.MenuId = SM.MenuId AND SM.MenuType=2");
                    break;
                case SysEnum.Enum_AuthorityType.ALL:
                    builder.AppendLine($@"
                                INNER JOIN Sys_AmRelated SAR ON SAR.AuthorityId = SAT.AuthorityId
                                INNER JOIN Sys_Menu SM ON SAR.MenuId = SM.MenuId");
                    break;
            }
            using (var conn = dapper.GetConnection)
            {
                var list = conn.Query<SysAuthority, SysMenu, SysAuthority>(builder.ToString(), (a, b) =>
               {
                   a.SysMenu = b;
                   return a;
               }, splitOn: "MenuId").ToList();
                return list;
            }

        }
        public List<SysMenu> GetSysBtn(Boolean isAdmin, List<SysRole> roles, string oid)
        {
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine($@"SELECT DISTINCT SAT.*, SM.*
                                FROM(
                                    SELECT SAT.*
                                    FROM Sys_Authority SAT WHERE DeleteSign={(int)SysEnum.Enum_DeleteSign.Sing_Deleted} ) SAT");
            if (!isAdmin)
            {
                var roleList = roles.Select(t => t.RoleId).ToList();
                if (roleList.Count() == 0)
                    roleList.Add(Guid.Empty.ToString());
                builder.AppendLine($@"INNER JOIN (
                                SELECT srar.AuthorityId
                                FROM Sys_Role srol, Sys_RaRelated srar
                                WHERE srol.RoleId = srar.RoleId
                                    AND srol.RoleId IN ('{string.Join("','", roleList)}')
                            ) t
                            ON t.AuthorityId = SAT.AuthorityId");
            }
            builder.AppendLine($@"
                    INNER JOIN Sys_AmRelated SAR ON SAR.AuthorityId = SAT.AuthorityId
                    INNER JOIN Sys_Menu SM ON SAR.MenuId = SM.MenuId AND SM.MenuType=2 AND SM.ParentMenuId='{oid}' order by SM.MenuSort");
            using (var conn = dapper.GetConnection)
            {
                var list = conn.Query<SysMenu>(builder.ToString()).ToList();
                return list;
            }

        }
    }
}
