using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Dapper;

namespace SANS.DAL.Implements
{
    public class SysRoleDAL : BaseDAL<SysRole>, ISysRoleDAL
    {
        public SysRoleDAL(DapperContext dapper, MyEFContext db) : base(dapper, db)
        {
        }
        /// <summary>
        /// 为角色添加功能菜单
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        public bool AddRoleMenu(string RoleId, List<SysMenu> menus)
        {
            try
            {
                StringBuilder builderSql = new StringBuilder(20);
                using (var conn = dapper.GetConnection)
                {
                    //删除角色权限表数据
                    builderSql.AppendLine($@"DELETE FROM Sys_RaRelated
                                        WHERE RaRelatedId IN (
                                        select * from 
                                          (SELECT T.RaRelatedId
                                          FROM Sys_RaRelated T, Sys_Authority T1
                                          WHERE T.AuthorityId = T1.AuthorityId
                                           AND T.RoleId = '{RoleId}'
                                           AND T1.AuthorityType = {(int)SysEnum.Enum_AuthorityType.Type_Menu}) as a
                                         );");
                    var sql = builderSql.ToString();
                    var list = conn.Query<SysAmRelated>($"SELECT * FROM Sys_AmRelated WHERE MenuId IN('{string.Join("','", menus.Select(t => t.MenuId))}')");
                    if (list != null && list.Count() > 0)
                    {
                        builderSql.AppendLine("INSERT INTO Sys_RaRelated(RaRelatedId,RoleId,AuthorityId)");
                        foreach (var item in list)
                        {
                            var strRaRelatedId = Guid.NewGuid().ToString();
                            builderSql.AppendLine($"SELECT '{strRaRelatedId}','{RoleId}','{item.AuthorityId}' UNION ALL");
                        }
                        var nindex = builderSql.ToString().LastIndexOf("UNION ALL");
                        if (nindex > -1)
                            sql = builderSql.ToString().Remove(nindex);
                    }
                    return conn.Execute(sql) >= 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(List<string> list)
        {
            throw new NotImplementedException();
        }
    }
}
