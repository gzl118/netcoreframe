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
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SANS.DbEntity.Models;

namespace SANS.WebApp.Filters
{
    /// <summary>
    /// 登陆验证过滤器(检测用户是否登录;不需要验证的时候加上NoVerificationLogin即可)
    /// </summary>
    public class VerificationLoginAttribute : Attribute, IResourceFilter
    {
        /// <summary>
        /// action执行之后
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
        /// <summary>
        /// action执行之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var isDefined = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                   .Any(a => a.GetType().Equals(typeof(NoVerificationLoginAttribute)));
            }
            if (isDefined) return;
            SysUser sysUsersLogin = context.HttpContext.Session.GetSession<SysUser>("UserLogin");
            if (sysUsersLogin == null)
            {
                context.HttpContext.Response.Redirect("/Home/BackLogin");
            }
            else if (sysUsersLogin.UserStatus == 0)
            {
                context.HttpContext.Response.Redirect("/Home/BackLogin");
            }
        }
    }
    /// <summary>
    /// 不需要验证登录的特性(使用方法:在方法或者控制器上加上该特性即可)
    /// </summary>
    public class NoVerificationLoginAttribute : Attribute
    {
    }
}
