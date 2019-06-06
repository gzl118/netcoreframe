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
using SANS.WebApp.Controllers;

namespace SANS.WebApp.Areas.SystemManager.Controllers
{
    [Area("System")]
    public class MenuController : BaseController
    {
        private readonly WholeInjection injection;
        public MenuController(WholeInjection injection)
        {
            this.injection = injection;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SelectMenuIcon()
        {
            return View();
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetMenuList()
        {
            var menuList = (List<SysMenuDto>)(injection.GetT<ISysMenuBLL>().GetMenusBy(injection.GetT<UserAccount>().GetUserInfo(), SysEnum.Enum_AuthorityType.ALL).Data);

            return JsonHelper.ObjectToJson(new ResponseModel
            {
                JsonData = menuList
            });
        }
        /// <summary>
        /// 添加/修改 一个菜单
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        public string AddOrEditMenu(SysMenuDto menuDto)
        {
            bool bResult = false;
            int menuCount = injection.GetT<ISysMenuBLL>().GetCount(t => t.MenuId.Equals(menuDto.MenuId));
            //添加
            if (menuCount == 0)
            {
                var AuthorityId = Guid.NewGuid().ToString();
                using (var tran = injection.GetT<MyEFContext>().Database.BeginTransaction())
                {
                    int i = 0;
                    //添加权限
                    bResult = injection.GetT<ISysAuthorityBLL>().Add(new SysAuthority
                    {
                        AuthorityType = (int)SysEnum.Enum_AuthorityType.Type_Menu,
                        AuthorityId = AuthorityId
                    });
                    if (bResult) i++;
                    bResult = injection.GetT<ISysAmRelatedBLL>().Add(new SysAmRelated
                    {
                        AmRelatedId = Guid.NewGuid().ToString(),
                        AuthorityId = AuthorityId,
                        MenuId = menuDto.MenuId
                    });
                    if (bResult) i++;
                    menuDto.CreateTime = DateTime.Now;
                    bResult = injection.GetT<ISysMenuBLL>().Add(AutoMapper.Mapper.Map<SysMenu>(menuDto));
                    if (bResult) i++;
                    if (i == 3)
                        tran.Commit();
                    else
                    {
                        tran.Rollback();
                        bResult = false;
                    }

                }
            }
            else
            {
                var menu = injection.GetT<ISysMenuBLL>().GetModels(t => t.MenuId.Equals(menuDto.MenuId)).SingleOrDefault();
                menu.MenuName = menuDto.MenuName;
                menu.MenuIcon = menuDto.MenuIcon;
                menu.MenuSort = menuDto.MenuSort;
                menu.MenuType = menuDto.MenuType;
                menu.Note = menuDto.Note;
                menu.MenuUrl = menuDto.MenuUrl;
                menu.ParentMenuId = menuDto.ParentMenuId;
                bResult = injection.GetT<ISysMenuBLL>().Update(menu);
            }
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                StateCode = bResult ? StatesCode.success : StatesCode.failure,
                Messages = bResult ? "添加成功" : "添加失败"
            });
        }
        /// <summary>
        /// 删除菜单(同时删除权限表对应的菜单权限)
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        public string DelMenu(string MenuId)
        {
            var messageModel = injection.GetT<ISysMenuBLL>().DelMenu(MenuId);
            return JsonHelper.ObjectToJson(new ResponseModel
            {
                Messages = messageModel.Message,
                StateCode = messageModel.Result ? StatesCode.success : StatesCode.failure
            });
        }

        #region 菜单控制
        public string GetMenuBtn(string oid)
        {
            var user = injection.GetT<UserAccount>().GetUserInfo();
            var lbtns = injection.GetT<ISysMenuBLL>().GetMenuBtns(user, oid);
            //var temp = string.Join("/", lbtns);
            return JsonHelper.ObjectToJson(new
            {
                code = 0,
                msg = "获取成功",
                data = lbtns
            });
        }
        #endregion

    }
}