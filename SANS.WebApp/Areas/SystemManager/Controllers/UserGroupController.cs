using System;
using System.Collections.Generic;
using System.Linq;
using DInjectionProvider;
using Microsoft.AspNetCore.Mvc;
using SANS.BLL;
using SANS.BLL.Interface;
using SANS.Common;
using SANS.DbEntity.Models;
using SANS.WebApp.Controllers;
using SANS.WebApp.Data;
using SANS.WebApp.Models;

namespace SANS.WebApp.Areas.SystemManager.Controllers
{
    [Area("System")]
    public class UserGroupController : BaseController
    {
        private readonly WholeInjection injection;
        public UserGroupController(WholeInjection injection)
        {
            this.injection = injection;
        }
        public IActionResult Index(string oid)
        {
            ViewBag.MenuOid = oid;
            return View();
        }
        public IActionResult UserGroupAdd(string UserGroupId)
        {

            ViewBag.UserGroupList = injection.GetT<ISysUserGroupBLL>().GetModels(t => ((!string.IsNullOrEmpty(UserGroupId) && !t.UserGroupId.Equals(UserGroupId)) || string.IsNullOrEmpty(UserGroupId)) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted));
            return View();
        }
        #region 获取分页也用户组数据
        /// <summary>
        /// 获取分页用户组数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchstr"></param>
        /// <returns></returns>
        public string GetUserGroupList(string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysUserGroupBLL>()
                 .GetUserGroupList(page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        #endregion
        #region 添加/修改用户组
        /// <summary>
        /// 添加/修改用户组
        /// </summary>
        /// <param name="userGroupDto"></param>
        /// <returns></returns>
        public string AddorEditUserGroup(SysUserGroupDto userGroupDto)
        {
            bool bResult = true;
            string sMessage = "保存成功";
            var userGroupBLL = injection.GetT<ISysUserGroupBLL>();
            var userGroup = AutoMapper.Mapper.Map<SysUserGroup>(userGroupDto);

            //添加
            if (string.IsNullOrEmpty(userGroupDto.UserGroupId))
            {
                if (userGroupBLL.GetCount(t => t.UserGroupName.Equals(userGroupDto.UserGroupName) && !t.UserGroupId.Equals(userGroupDto.UserGroupId) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted)) > 0)
                {
                    sMessage = "用户组名称不能重复";
                    bResult = !bResult;
                }
                else
                {
                    userGroup.UserGroupId = Guid.NewGuid().ToString();
                    userGroup.DeleteSign = (Int32)SysEnum.Enum_DeleteSign.Sing_Deleted;
                    userGroup.CreateUserId = injection.GetT<UserAccount>().GetUserInfo().UserId;
                    userGroup.CreateTime = DateTime.Now;
                    bResult = userGroupBLL.Add(userGroup);
                }
            }
            else//修改
            {
                var userGroup_Edit = userGroupBLL.GetModels(t => t.UserGroupId.Equals(userGroup.UserGroupId)).SingleOrDefault();
                userGroup_Edit.UserGroupName = userGroup.UserGroupName;
                userGroup_Edit.ParentUserGroupId = userGroup.ParentUserGroupId;
                userGroup_Edit.EditTime = DateTime.Now;
                userGroup_Edit.Note = userGroup.Note;
                bResult = userGroupBLL.Update(userGroup_Edit);
            }

            return JsonHelper.ObjectToJson(new ResponseModel
            {
                StateCode = bResult ? StatesCode.success : StatesCode.failure,
                Messages = sMessage
            });
        }
        #endregion
        #region 批量删除用户组
        /// <summary>
        /// 批量删除用户组
        /// </summary>
        /// <param name="UserGroupId"></param>
        /// <returns></returns>
        public string DelUserGroup(List<string> UserGroupId)
        {
            var resModel = new ResponseModel();
            //此处删除进行逻辑删除
            MessageModel model = injection.GetT<ISysUserGroupBLL>().DelUserGroup(UserGroupId);
            resModel.StateCode = model.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = model.Message;
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
    }
}