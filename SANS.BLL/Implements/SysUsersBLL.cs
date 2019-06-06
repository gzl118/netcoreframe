using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using SANS.BLL.Interface;
using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using SANS.DbEntity.Views;
using System.Text;
using AutoMapper;
using SANS.Common;

namespace SANS.BLL.Implements
{
    public class SysUserBLL : BaseBLL<SysUser>, ISysUserBLL
    {
        private ISysUserDAL usersDAL;
        /// <summary>
        /// 用于实例化父级，usersDAL变量
        /// </summary>
        /// <param name="usersDAL"></param>
        public SysUserBLL(ISysUserDAL usersDAL) : base(usersDAL)
        {
            this.usersDAL = usersDAL;
        }
        #region 获取需要登录的用户所有信息
        /// <summary>
        /// 获取需要登录的用户所有信息
        /// </summary>
        /// <returns></returns>
        public MessageModel GetLoginSysUser(SysUser user)
        {
            return new MessageModel
            {
                Data = usersDAL.SetLoginSysUser(user)
            };
        }
        #endregion
        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        public MessageModel GetUserList(string page, string limit, string searchstr)
        {
            var messageModel = new MessageModel();
            var PageModel = new PageModel();
            Expression<Func<SysUser, bool>> expression = t => (string.IsNullOrEmpty(searchstr) || t.UserName.Contains(searchstr) ||
                t.UserNikeName.Contains(searchstr) ||
                t.UserPhone.Contains(searchstr) ||
                t.UserQq.Contains(searchstr) ||
                t.UserWx.Contains(searchstr) ||
                t.UserEmail.Contains(searchstr)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted);
            PageModel.RowCount = usersDAL.GetCount(expression);
            int iBeginRow = Convert.ToInt32(limit) * (Convert.ToInt32(page) - 1), iEndRow = Convert.ToInt32(page) * Convert.ToInt32(limit);
            var list = usersDAL.SqlQuery<SysUserDto>($@"                                           
                                    SELECT  T1.*
                                    FROM Sys_User T1
                                    WHERE (T1.UserNikeName LIKE '%{searchstr}%'
                                            OR T1.UserPhone LIKE '%{searchstr}%'
                                            OR T1.UserQq LIKE '%{searchstr}%'
                                            OR T1.UserWx LIKE '%{searchstr}%'
                                            OR T1.UserEmail LIKE '%{searchstr}%')
                                        AND T1.DeleteSign = 1 order by T1.CREATETIME 
                                limit  {iBeginRow} ,  {limit}");
            PageModel.Data = list;
            messageModel.Data = PageModel;
            return messageModel;
        }
        public MessageModel GetUserListByUsers(List<string> userids)
        {
            var messageModel = new MessageModel();
            var ul = usersDAL.GetModels(f => userids.Contains(f.UserId));
            messageModel.Data = ul.ToList();
            messageModel.Message = "成功";
            return messageModel;
        }
        #endregion
        #region 添加/编辑用户
        /// <summary>
        /// 添加/编辑用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="userId">当前登录用户id</param>
        /// <returns></returns>
        public MessageModel AddorEditUser(SysUserDto userDto, string userId)
        {
            var messageModel = new MessageModel();
            var user = Mapper.Map<SysUser>(userDto);
            int intcount = usersDAL.GetCount(t => t.UserName.Equals(user.UserName));
            //添加
            if (string.IsNullOrEmpty(user.UserId))
            {
                if (intcount == 0)
                {
                    user.UserId = Guid.NewGuid().ToString();
                    //创建人id
                    user.CreateUserId = userId;
                    //默认密码
                    user.UserPwd = MD5Encrypt.Get32MD5One("123456");
                    user.CreateTime = DateTime.Now;
                    //user.EditTime = DateTime.Now;
                    //添加用户
                    if (Add(user))
                    {
                        messageModel.Message = "添加成功";
                    }
                    else
                    {
                        messageModel.Message = "添加失败";
                        messageModel.Result = false;
                    }
                }
                else
                {
                    messageModel.Message = "用户名已经被注册";
                    messageModel.Result = false;
                }
            }
            //修改
            else
            {
                var user_edit = GetModels(t => t.UserId.Equals(user.UserId)).SingleOrDefault();
                user_edit.UserName = user.UserName;
                user_edit.UserNikeName = user.UserNikeName;
                user_edit.UserSex = user.UserSex;
                user_edit.UserBirthday = user.UserBirthday;
                user_edit.UserEmail = user.UserEmail;
                user_edit.UserQq = user.UserQq;
                user_edit.UserWx = user.UserWx;
                user_edit.UserPhone = user.UserPhone;
                user_edit.UserGroupId = user.UserGroupId;
                user_edit.Note = user.Note;
                user_edit.EditTime = DateTime.Now;
                user_edit.UasSub = user.UasSub;
                user_edit.UserStatus = user.UserStatus;
                if (Update(user_edit))
                {
                    messageModel.Message = "修改成功";
                }
                else
                {
                    messageModel.Message = "修改失败";
                }

            }

            return messageModel;
        }
        #endregion
        #region 删除用户(批量删除)
        /// <summary>
        /// 删除用户(批量删除)
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public MessageModel DelUser(IEnumerable<string> UserId)
        {
            var model = new MessageModel();
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine(@"DELETE FROM Sys_User  WHERE UserId IN (");
            //builder.AppendLine(value: $"UPDATE SYS_USER SET DeleteSign={(Int32)SysEnum.Enum_DeleteSign.Sign_Undeleted},DeleteTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE USERID IN (");
            builder.AppendLine($"'{String.Join("','", UserId)}')");
            builder.AppendLine($@";DELETE FROM Sys_UrRelated  WHERE UserId IN (");
            builder.AppendLine($"'{String.Join("','", UserId)}')");
            bool bResult = ExecuteSql(builder.ToString()) > 0;
            model.Result = bResult;
            model.Message = bResult ? "删除成功" : "删除失败";
            return model;
        }
        #endregion

        #region 启用禁用用户
        public MessageModel EnabledUser(IEnumerable<string> UserId, int nStatus)
        {
            var temp = nStatus == 0 ? "禁用" : "启用";
            var model = new MessageModel();
            StringBuilder builder = new StringBuilder(20);
            builder.AppendLine($@"UPDATE Sys_User Set UserStatus={nStatus} WHERE UserId IN (");
            builder.AppendLine($"'{String.Join("','", UserId)}')");
            bool bResult = ExecuteSql(builder.ToString()) > 0;
            model.Result = bResult;
            model.Message = bResult ? temp + "成功" : temp + "失败";
            return model;
        }
        #endregion
    }
}
