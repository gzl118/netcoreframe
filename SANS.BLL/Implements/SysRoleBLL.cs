using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SANS.BLL.Interface;
using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using SANS.DbEntity.Views;

namespace SANS.BLL.Implements
{
    public class SysRoleBLL : BaseBLL<SysRole>, ISysRoleBLL
    {
        private readonly ISysRoleDAL sysRoleDAL;
        private readonly ISysUserDAL sysUserDAL;
        private readonly ISysUrRelatedDAL sysUrRelatedDAL;
        public readonly ISysUserGroupDAL sysUserGroupDAL;
        /// <summary>
        /// 用于实例化父级，sysRoleDAL
        /// </summary>
        /// <param name="sysRoleDAL"></param>
        public SysRoleBLL(ISysRoleDAL sysRoleDAL, ISysUserDAL sysUserDAL, ISysUrRelatedDAL sysUrRelatedDAL, ISysUserGroupDAL sysUserGroupDAL) : base(sysRoleDAL)
        {
            this.sysRoleDAL = sysRoleDAL;
            this.sysUserDAL = sysUserDAL;
            this.sysUrRelatedDAL = sysUrRelatedDAL;
            this.sysUserGroupDAL = sysUserGroupDAL;
        }
        #region 删除角色(批量删除)
        /// <summary>
        /// 删除用户(批量删除)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public MessageModel DelRole(IEnumerable<string> RoleId)
        {
            var model = new MessageModel();
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine(value: $"UPDATE Sys_Role SET DeleteSign={(Int32)SysEnum.Enum_DeleteSign.Sign_Undeleted},DeleteTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE RoleId IN (");
            builder.AppendLine($"'{String.Join("','", RoleId)}')");
            bool bResult = ExecuteSql(builder.ToString()) > 0;
            model.Result = bResult;
            model.Message = bResult ? "删除成功" : "删除失败";
            return model;
        }
        #endregion
        #region 获取角色列表
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        public MessageModel GetRoleList(string page, string limit, string searchstr)
        {
            var query = sysRoleDAL.GetModelsByPage(Convert.ToInt32(limit), Convert.ToInt32(page), false, t => t.CreateTime,
                t => t.RoleName.Contains(searchstr) || string.IsNullOrEmpty(searchstr)
                && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted));
            return new MessageModel
            {
                Data = new PageModel
                {
                    RowCount = query.Count(),
                    Data = AutoMapper.Mapper.Map<List<SysRole>>(query.ToList())
                }
            };
        }
        #endregion
        #region 为角色添加功能菜单
        /// <summary>
        /// 为角色添加功能菜单
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="menuDtos"></param>
        /// <returns></returns>
        public MessageModel AddRoleMenu(string RoleId, List<SysMenuDto> menuDtos)
        {
            return new MessageModel
            {
                Result = sysRoleDAL.AddRoleMenu(RoleId, AutoMapper.Mapper.Map<List<SysMenu>>(menuDtos))
            };
        }
        #endregion
        #region 获取角色所分配的用户
        /// <summary>
        /// 获取角色所分配的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public MessageModel GetRoleUserList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var PageModel = new PageModel();
            var urRelateds = sysUrRelatedDAL.GetModels(t => t.RoleId.Equals(RoleId)).ToList();
            Expression<Func<SysUser, bool>> expression = t => (string.IsNullOrEmpty(searchstr) || t.UserName.Contains(searchstr) ||
                t.UserNikeName.Contains(searchstr) ||
                t.UserPhone.Contains(searchstr) ||
                t.UserQq.Contains(searchstr) ||
                t.UserWx.Contains(searchstr) ||
                t.UserEmail.Contains(searchstr)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted)
                && urRelateds.Any(x => x.UserId == t.UserId);
            PageModel.RowCount = sysUserDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = sysUserDAL.SqlQuery<SysUserDto>($@"
                                            SELECT T1.*
                                    FROM Sys_User T1
                                    WHERE (T1.UserNikeName LIKE '%{searchstr}%'
                                            OR T1.UserPhone LIKE '%{searchstr}%'
                                            OR T1.UserQq LIKE '%{searchstr}%'
                                            OR T1.UserWx LIKE '%{searchstr}%'
                                            OR T1.UserEmail LIKE '%{searchstr}%')
                                        AND T1.DeleteSign = 1
                                        AND T1.UserId IN('{String.Join("','", urRelateds.Count == 0 ? new List<string>() : urRelateds.Select(t => t.UserId))}')
                                      order by T1.CREATETIME
                                limit   {iBeginRow} , {limit}");
            PageModel.Data = list;
            messageModel.Data = PageModel;
            return messageModel;
        }
        /// <summary>
        /// 获取角色不包含分配的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public MessageModel GetRoleNotUserList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var PageModel = new PageModel();
            var urRelateds = sysUrRelatedDAL.GetModels(t => t.RoleId.Equals(RoleId)).ToList();
            Expression<Func<SysUser, bool>> expression = t => (string.IsNullOrEmpty(searchstr) || t.UserName.Contains(searchstr) ||
                t.UserNikeName.Contains(searchstr) ||
                t.UserPhone.Contains(searchstr) ||
                t.UserQq.Contains(searchstr) ||
                t.UserWx.Contains(searchstr) ||
                t.UserEmail.Contains(searchstr)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted)
                && !urRelateds.Exists(x => x.UserId == t.UserId);
            PageModel.RowCount = sysUserDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = sysUserDAL.SqlQuery<SysUserDto>($@"
                                            SELECT T1.*
                                    FROM Sys_User T1
                                    WHERE (T1.UserNikeName LIKE '%{searchstr}%'
                                            OR T1.UserPhone LIKE '%{searchstr}%'
                                            OR T1.UserQq LIKE '%{searchstr}%'
                                            OR T1.UserWx LIKE '%{searchstr}%'
                                            OR T1.UserEmail LIKE '%{searchstr}%')
                                        AND T1.DeleteSign = 1
                                        AND T1.UserId NOT IN('{String.Join("','", urRelateds.Count == 0 ? new List<string>() : urRelateds.Select(t => t.UserId))}')
                                      order by T1.CREATETIME
                                limit   {iBeginRow} , {limit}");
            PageModel.Data = list;
            messageModel.Data = PageModel;
            return messageModel;
        }
        #endregion
        #region 为角色分配用户
        /// <summary>
        /// 为角色分配用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        public MessageModel AssignmentRoleUser(string RoleId, List<string> UserIds)
        {
            StringBuilder builderDelSql = new StringBuilder(20);
            //builderDelSql.AppendLine($"DELETE FROM Sys_UrRelated WHERE RoleId = '{RoleId}';");
            builderDelSql.AppendLine("INSERT INTO Sys_UrRelated(UrRelatedId,UserId,RoleId) ");
            foreach (var item in UserIds)
            {
                var UrRelatedId = Guid.NewGuid().ToString();
                builderDelSql.AppendLine($"SELECT '{UrRelatedId}','{item}', '{RoleId}' UNION ALL");
            }
            var sql = builderDelSql.ToString().Remove(builderDelSql.ToString().LastIndexOf("UNION ALL"));
            bool bReuslt = sysRoleDAL.ExecuteSql(sql) > 0;
            return new MessageModel
            {
                Message = bReuslt ? "分配成功" : "分配失败",
                Result = bReuslt
            };
        }
        #endregion
        #region 取消用户角色
        /// <summary>
        /// 取消用户角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        public MessageModel CancelUserAssignment(string RoleId, List<string> UserIds)
        {
            bool bReuslt = sysRoleDAL.ExecuteSql($"DELETE FROM Sys_UrRelated WHERE RoleId='{RoleId}' AND UserId IN('{String.Join("','", UserIds)}')") > 0;
            return new MessageModel
            {
                Message = bReuslt ? "取消授权成功" : "取消授权失败",
                Result = bReuslt
            };
        }
        #endregion
        #region 获取角色所分配的用户组
        /// <summary>
        /// 获取角色所分配的用户组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public MessageModel GetRoleUserGroupList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var PageModel = new PageModel();
            var urRelateds = sysUrRelatedDAL.SqlQuery<SysUgrRelated>($"SELECT * FROM Sys_UgrRelated WHERE RoleId='{RoleId}'").ToList();
            Expression<Func<SysUserGroup, bool>> expression = t => (string.IsNullOrEmpty(searchstr) ||
                t.UserGroupName.Contains(searchstr)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted)
                && urRelateds.Any(x => x.UserGroupId == t.UserGroupId);
            PageModel.RowCount = sysUserGroupDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = sysUserDAL.SqlQuery<SysUserGroupDto>($@"                                     
                                    SELECT T1.*
                                    FROM Sys_UserGroup T1
                                    WHERE T1.UserGroupName LIKE '%{searchstr}%'
                                        AND T1.DeleteSign = 1
                                        AND T1.UserGroupId IN('{String.Join("','", urRelateds.Count == 0 ? new List<string>() : urRelateds.Select(t => t.UserGroupId))}')
                                ORDER BY T1.CREATETIME
                                limit  {iBeginRow},{limit} ");
            PageModel.Data = list;
            messageModel.Data = PageModel;
            return messageModel;
        }
        /// <summary>
        /// 获取角色不包含的用户组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public MessageModel GetRoleNotUserGroupList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var PageModel = new PageModel();
            var urRelateds = sysUrRelatedDAL.SqlQuery<SysUgrRelated>($"SELECT * FROM Sys_UgrRelated WHERE RoleId='{RoleId}'").ToList();
            Expression<Func<SysUserGroup, bool>> expression = t => (string.IsNullOrEmpty(searchstr) ||
                t.UserGroupName.Contains(searchstr)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted)
                && !urRelateds.Exists(x => x.UserGroupId == t.UserGroupId);
            PageModel.RowCount = sysUserGroupDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = sysUserDAL.SqlQuery<SysUserGroupDto>($@"                                     
                                    SELECT T1.*
                                    FROM Sys_UserGroup T1
                                    WHERE T1.UserGroupName LIKE '%{searchstr}%'
                                        AND T1.DeleteSign = 1
                                        AND T1.UserGroupId NOT IN('{String.Join("','", urRelateds.Count == 0 ? new List<string>() : urRelateds.Select(t => t.UserGroupId))}')
                                ORDER BY T1.CREATETIME
                                limit  {iBeginRow},{limit} ");
            PageModel.Data = list;
            messageModel.Data = PageModel;
            return messageModel;
        }
        #endregion
        #region 为角色分配用户组
        /// <summary>
        /// 为角色分配用户组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserGroupIds"></param>
        /// <returns></returns>
        public MessageModel AssignmentRoleUserGroup(string RoleId, List<string> UserGroupIds)
        {
            StringBuilder builderDelSql = new StringBuilder(20);
            //builderDelSql.AppendLine($"DELETE FROM Sys_UgrRelated WHERE RoleId = '{RoleId}';");
            builderDelSql.AppendLine("INSERT INTO Sys_UgrRelated(UgrRelatedId,UserGroupId,RoleId) ");
            foreach (var item in UserGroupIds)
            {
                var UgrRelatedId = Guid.NewGuid().ToString();
                builderDelSql.AppendLine($"SELECT '{UgrRelatedId}','{item}', '{RoleId}' UNION ALL");
            }
            var sql = builderDelSql.ToString().Remove(builderDelSql.ToString().LastIndexOf("UNION ALL"));
            bool bReuslt = sysRoleDAL.ExecuteSql(sql) > 0;
            return new MessageModel
            {
                Message = bReuslt ? "分配成功" : "分配失败",
                Result = bReuslt
            };
        }
        #endregion
        #region 取消用户组角色
        /// <summary>
        /// 取消用户组角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserGroupIds"></param>
        /// <returns></returns>
        public MessageModel CancelUserGroupAssignment(string RoleId, List<string> UserGroupIds)
        {
            bool bReuslt = sysRoleDAL.ExecuteSql($"DELETE FROM Sys_UgrRelated WHERE RoleId='{RoleId}' AND UserGroupId IN('{String.Join("','", UserGroupIds)}')") > 0;
            return new MessageModel
            {
                Message = bReuslt ? "取消授权成功" : "取消授权失败",
                Result = bReuslt
            };
        }
        #endregion
    }
}
