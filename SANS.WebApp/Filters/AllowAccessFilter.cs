using System.Linq;
using System.Net.Http;
using System.Text;
using SANS.Common;
using SANS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SANS.Config;

namespace SANS.WebApp.Filters
{
    /// <summary>
    /// 访问许可验证
    /// </summary>
    public class AllowAccessFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var rquired = SiteConfig.GetSite("CustomConfiguration", "IsVerifyToken").ToLower() == "true";
            if (!rquired) return;
            //验证访问Ip地址
            var accessIp = GetUserIp(context);
            var allowAccessIp = SiteConfig.GetSite("CustomConfiguration", "AllowAccessIp");
            if (allowAccessIp.Contains(accessIp)) return;
            context.Result = new ContentResult() { Content = "非法的访问请求:" + accessIp };
        }
        private string GetUserIp(ActionExecutingContext context)
        {
            var ip = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}