#region 版权声明
/**************************************************************** 
 * 作    者：周黎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/6/8 21:41:11 
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
using Microsoft.AspNetCore.Mvc;
using DInjectionProvider;
using SANS.WebApp.Data;
using SANS.WebApp.Comm;
using SANS.WebApp.Filters;

namespace SANS.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly WholeInjection injection;
        public HomeController(WholeInjection injection)
        {
            this.injection = injection;
        }
        public IActionResult Index()
        {
            ViewBag.SysName = ProjectConfig.SysName;
            var User = injection.GetT<UserAccount>().GetUserInfo();
            return View(User);
        }
        public IActionResult Welcome()
        {
            return View();
        }
        [NoVerificationLogin]
        public IActionResult BackLogin()
        {
            return View();
        }
    }
}
