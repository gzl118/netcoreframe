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
    public class UserController : BaseController
    {
        private readonly WholeInjection injection;
        public UserController(WholeInjection injection)
        {
            this.injection = injection;
        }
        public IActionResult Index(string oid)
        {
            ViewBag.MenuOid = oid;
            return View();
        }
        public IActionResult UserAdd()
        {
            ViewBag.UserGroupList = injection.GetT<ISysUserGroupBLL>().GetModels(t => t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted));
            return View();
        }
        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="page">第几页</param>
        /// <param name="limit">页容量</param>
        /// <param name="searchstr">搜索内容</param>
        /// <returns></returns>
        public string GetUserList(string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysUserBLL>()
                .GetUserList(page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        #endregion
        #region 添加/编辑用户
        /// <summary>
        /// 添加/编辑用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public string AddorEditUser(SysUserDto userDto)
        {
            var resModel = new ResponseModel();
            var userLogin = injection.GetT<Data.UserAccount>().GetUserInfo();
            var mModel = injection.GetT<ISysUserBLL>().AddorEditUser(userDto, userLogin.UserId);
            resModel.StateCode = mModel.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = mModel.Message;
            resModel.JsonData = mModel.Data;
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
        #region 禁用/启用用户
        /// <summary>
        /// 禁用/启用用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string DisableUser(List<string> UserId, int UserStatus)
        {
            var resModel = new ResponseModel();
            MessageModel model = injection.GetT<ISysUserBLL>().EnabledUser(UserId, UserStatus);
            resModel.StateCode = model.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = model.Message;
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
        #region 批量删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string DelUser(List<string> UserId)
        {
            var resModel = new ResponseModel();
            //此处删除进行逻辑删除
            MessageModel model = injection.GetT<ISysUserBLL>().DelUser(UserId);
            resModel.StateCode = model.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = model.Message;
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
    }
}