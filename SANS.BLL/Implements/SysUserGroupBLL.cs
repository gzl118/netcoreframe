using AutoMapper;
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
    public class SysUserGroupBLL : BaseBLL<SysUserGroup>, ISysUserGroupBLL
    {
        private ISysUserGroupDAL userGroupDAL;
        /// <summary>
        /// 用于实例化父级，userGroupDAL
        /// </summary>
        /// <param name="userGroupDAL"></param>
        public SysUserGroupBLL(ISysUserGroupDAL userGroupDAL) : base(userGroupDAL)
        {
            this.userGroupDAL = userGroupDAL;
        }
        #region 获取用户组列表
        /// <summary>
        /// 获取用户组列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        public MessageModel GetUserGroupList(string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var pageModel = new PageModel();
            Expression<Func<SysUserGroup, bool>> expression = t => (string.IsNullOrEmpty(searchstr) || t.UserGroupName.Contains(searchstr)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted);
            pageModel.RowCount = userGroupDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = userGroupDAL.SqlQuery<SysUserGroupDto>($@"SELECT 
                             T1.UserGroupName,
                             T1.ParentUserGroupId,
                            T2.UserGroupName ParentUserGroupName, T1.CreateTime, T1.Note 
                            FROM Sys_UserGroup T1
                            LEFT JOIN Sys_UserGroup T2
                            ON T1.ParentUserGroupId=T2.UserGroupId
                            WHERE  (T1.UserGroupName LIKE '%{searchstr}%' 
                            OR T2.UserGroupName LIKE '%{searchstr}%')
                            AND T1.DeleteSign=1 order by T1.CREATETIME 
                            limit  {iBeginRow} ,  {limit} ");

            pageModel.Data = list;
            messageModel.Data = pageModel;
            return messageModel;
        }
        #endregion
        #region 删除用户组(批量删除)
        /// <summary>
        /// 删除用户(批量删除)
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <returns></returns>
        public MessageModel DelUserGroup(IEnumerable<string> UserGroupId)
        {
            var model = new MessageModel();
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine(value: $"UPDATE SYS_USERGroup SET DeleteSign={(Int32)SysEnum.Enum_DeleteSign.Sign_Undeleted},DeleteTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE UserGroupId IN (");
            builder.AppendLine($"'{String.Join("','", UserGroupId)}')");
            bool bResult = ExecuteSql(builder.ToString()) > 0;
            model.Result = bResult;
            model.Message = bResult ? "删除成功" : "删除失败";
            return model;
        }
        #endregion
    }
}
