using System;
using System.Collections.Generic;
using System.Linq;
using DInjectionProvider;
using Microsoft.AspNetCore.Mvc;
using SANS.Common;
using SANS.BLL;
using SANS.BLL.Interface;
using SANS.DbEntity.Models;
using SANS.WebApp.Data;
using SANS.WebApp.Models;
using Microsoft.Extensions.Logging;
using SANS.WebApp.Controllers;

namespace SANS.WebApp.Areas.SystemManager.Controllers
{
    [Area("System")]
    public class RoleController : BaseController
    {
        private readonly WholeInjection injection;
        private readonly ILogger<RoleController> logger;
        public RoleController(WholeInjection injection, ILogger<RoleController> _logger)
        {
            this.injection = injection;
            this.logger = _logger;
        }
        #region 视图部分
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string oid)
        {
            logger.LogInformation("访问角色视图页面!");
            ViewBag.MenuOid = oid;
            return View();
        }
        /// <summary>
        /// 添加角色视图
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleAdd() => View();
        /// <summary>
        /// 角色分配给用户
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleAssignmentUser(string oid)
        {
            ViewBag.MenuOid = oid;
            return View();
        }
        /// <summary>
        /// 角色分配给用户组
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleAssignmentUserGroup(string oid)
        {
            ViewBag.MenuOid = oid;
            return View();
        }
        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        public IActionResult SelectUser() => View();
        /// <summary>
        /// 选择用户组
        /// </summary>
        /// <returns></returns>
        public IActionResult SelectUserGroup() => View();

        public IActionResult StaticZLView() => View();
        #endregion
        #region 获取分页角色数据
        /// <summary>
        /// 获取分页角色数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchstr"></param>
        /// <returns></returns>
        public string GetRoleList(string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysRoleBLL>()
                 .GetRoleList(page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        #endregion
        #region 添加/修改角色
        /// <summary>
        /// 添加/修改角色
        /// </summary>
        /// <param name="userGroupDto"></param>
        /// <param name="objCheckMenus">角色的菜单权限</param>
        /// <returns></returns>
        public string AddorEditRole(string sjson)
        {
            var model = JsonHelper.JsonToObject<SysRoleVO>(sjson);
            if (model == null)
            {
                return JsonHelper.ObjectToJson(new ResponseModel
                {
                    StateCode = StatesCode.failure,
                    Messages = "数据解析失败"
                });
            }
            SysRoleDto roleDto = model.roleDto;
            List<SysMenuDto> objCheckMenus = model.objCheckMenus;
            bool bResult = true;
            string sMessage = "保存成功";
            var roleBLL = injection.GetT<ISysRoleBLL>();
            var role = AutoMapper.Mapper.Map<SysRole>(roleDto);

            //添加
            if (string.IsNullOrEmpty(role.RoleId))
            {
                if (roleBLL.GetCount(t => t.RoleName.Equals(role.RoleName) && !t.RoleId.Equals(role.RoleId) && t.DeleteSign.Equals((int)SysEnum.Enum_DeleteSign.Sing_Deleted)) > 0)
                {
                    sMessage = "角色名称不能重复";
                    bResult = !bResult;
                }
                else
                {
                    role.RoleId = Guid.NewGuid().ToString();
                    role.DeleteSign = (Int32)SysEnum.Enum_DeleteSign.Sing_Deleted;
                    role.CreateUserId = injection.GetT<UserAccount>().GetUserInfo().UserId;
                    role.CreateTime = DateTime.Now;
                    bResult = roleBLL.Add(role);
                    bResult = roleBLL.AddRoleMenu(role.RoleId, objCheckMenus).Result;
                }
            }
            else//修改
            {
                var userRole_Edit = roleBLL.GetModels(t => t.RoleId.Equals(role.RoleId)).SingleOrDefault();
                userRole_Edit.RoleName = role.RoleName;
                userRole_Edit.EditTime = DateTime.Now;
                userRole_Edit.Note = role.Note;
                bResult = roleBLL.Update(userRole_Edit);
                bResult = roleBLL.AddRoleMenu(userRole_Edit.RoleId, objCheckMenus).Result;
            }

            return JsonHelper.ObjectToJson(new ResponseModel
            {
                StateCode = bResult ? StatesCode.success : StatesCode.failure,
                Messages = sMessage
            });
        }
        #endregion
        #region 批量删除角色
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public string DelRole(List<string> RoleId)
        {
            var resModel = new ResponseModel();
            //此处删除进行逻辑删除
            MessageModel model = injection.GetT<ISysRoleBLL>().DelRole(RoleId);
            resModel.StateCode = model.Result ? StatesCode.success : StatesCode.failure;
            resModel.Messages = model.Message;
            return JsonHelper.ObjectToJson(resModel);
        }
        #endregion
        #region 获取角色的功能菜单
        /// <summary>
        /// 获取角色的功能菜单
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public string GetRoleMenuList(string RoleId)
        {
            if (string.IsNullOrEmpty(RoleId))
            {
                RoleId = Guid.Empty.ToString();
            }
            //用户可以操作的菜单
            var menuList = (List<SysMenuDto>)(injection.GetT<ISysMenuBLL>().GetMenusBy(injection.GetT<UserAccount>().GetUserInfo(), SysEnum.Enum_AuthorityType.ALL).Data);
            //角色所拥有的菜单
            var roleMenuList = RoleId.Equals(Guid.Empty.ToString()) ? new List<SysMenuDto>() : ((List<SysMenuDto>)injection.GetT<ISysMenuBLL>().GetRoleMenuList(RoleId).Data);
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                JsonData = new
                {
                    MenuList = menuList,
                    RoleMenuList = roleMenuList
                }
            });
        }
        #endregion
        #region 获取角色所分配的用户
        public string GetRoleUserList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().GetRoleUserList(RoleId, page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        public string GetRoleNotUserList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().GetRoleNotUserList(RoleId, page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        #endregion
        #region 为角色分配用户
        /// <summary>
        /// 为角色分配用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        public string AssignmentRoleUser(string RoleId, List<string> UserIds)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().AssignmentRoleUser(RoleId, UserIds);
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                Messages = messageModel.Message,
                StateCode = messageModel.Result ? StatesCode.success : StatesCode.failure
            });
        }
        #endregion
        #region 取消用户角色
        /// <summary>
        /// 取消用户角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserIds"></param>
        public string CancelUserAssignment(string RoleId, List<string> UserIds)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().CancelUserAssignment(RoleId, UserIds);
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                Messages = messageModel.Message,
                StateCode = messageModel.Result ? StatesCode.success : StatesCode.failure
            });
        }
        #endregion
        #region 获取角色所分配的用户组
        /// <summary>
        /// 获取角色所分配的用户组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchstr"></param>
        /// <returns></returns>
        public string GetRoleUserGroupList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().GetRoleUserGroupList(RoleId, page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        /// <summary>
        /// 获取角色不包含的用户组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="searchstr"></param>
        /// <returns></returns>
        public string GetRoleNotUserGroupList(string RoleId, string page, string limit, string searchstr)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().GetRoleNotUserGroupList(RoleId, page, limit, searchstr);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                count = messageModel.Data.RowCount,
                data = messageModel.Data.Data
            });
        }
        #endregion
        #region 为角色分配用户组
        /// <summary>
        /// 为角色分配用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserGroupIds"></param>
        /// <returns></returns>
        public string AssignmentRoleUserGroup(string RoleId, List<string> UserGroupIds)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().AssignmentRoleUserGroup(RoleId, UserGroupIds);
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                Messages = messageModel.Message,
                StateCode = messageModel.Result ? StatesCode.success : StatesCode.failure
            });
        }
        #endregion
        #region 取消用户组角色
        /// <summary>
        /// 取消用户角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="UserGroupIds"></param>
        public string CancelUserGroupAssignment(string RoleId, List<string> UserGroupIds)
        {
            var messageModel = injection.GetT<ISysRoleBLL>().CancelUserGroupAssignment(RoleId, UserGroupIds);
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                Messages = messageModel.Message,
                StateCode = messageModel.Result ? StatesCode.success : StatesCode.failure
            });
        }
        #endregion
    }
}