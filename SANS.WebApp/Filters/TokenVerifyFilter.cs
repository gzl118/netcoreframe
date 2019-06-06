using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using SANS.Common;
using SANS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SANS.Config;

namespace SANS.WebApp.Filters
{
    /// <summary>
    /// 用户令牌验证
    /// </summary>
    public class TokenVerifyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            //throw new NotImplementedException();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            //是否启用令牌验证
            var rquired = SiteConfig.GetSite("CustomConfiguration", "IsVerifyToken").ToLower() == "true";
            if (!rquired) return;

            var rquireduas = SiteConfig.GetSite("CustomConfiguration", "IsVerifyUasToken").ToLower() == "true";

            //是否匿名访问验证
            var isDefined = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                   .Any(a => a.GetType().Equals(typeof(AnonymousFilter)));
            }
            if (isDefined) return;
            JsonSerializerSettings setting = Util.GetJsonSetting();
            try
            {
                var requestHead = context.HttpContext.Request.Headers;
                var ReqUserToken = requestHead["ReqUserToken"];
                var ReqUserId = requestHead["ReqUserId"];
                var ReqDateExpire = requestHead["ReqDateExpire"];
                var ReqUasToken = requestHead["ReqUasToken"];
                var ReqUasSub = requestHead["ReqUasSub"];
                UserTokenModel token = getUserToken(ReqUserToken, ReqUserId, ReqDateExpire);
                if (token == null || string.IsNullOrEmpty(token.ReqUserId) || string.IsNullOrEmpty(token.ReqUserToken))
                {
                    var requestQuery = context.HttpContext.Request.Query;
                    ReqUserToken = requestQuery["ReqUserToken"];
                    ReqUserId = requestQuery["ReqUserId"];
                    ReqDateExpire = requestQuery["ReqDateExpire"];
                    ReqUasToken = requestQuery["ReqUasToken"];
                    ReqUasSub = requestQuery["ReqUasSub"];
                    token = getUserToken(ReqUserToken, ReqUserId, ReqDateExpire);
                }
                if (token == null)
                {
                    token = new UserTokenModel { ReqUserId = string.Empty };
                }
                if (token.ReqUserId == string.Empty || string.IsNullOrEmpty(token.ReqUserToken))
                {
                    context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.businessfail, Message = "无效的令牌请求参数" }, setting);
                }
                else
                {
                    //验证用户令牌
                    var message = string.Empty;
                    try
                    {
                        if (rquireduas)
                        {
                            //Log.Log4netHelper.Error(this, "ReqUasToken:" + ReqUasToken + ">>ReqUasSub:" + ReqUasSub);
                            if (string.IsNullOrEmpty(ReqUasToken) || string.IsNullOrEmpty(ReqUasSub))
                                message = "UasToken验证失败:ReqUasToken或ReqUasSub为空";
                            else if (!RedisOperation.RedisHelper.Default.KeyExists(ReqUasSub))
                                message = "UasToken验证失败:Redis中不存在Key";
                            else
                            {
                                var redisUasToken = RedisOperation.RedisHelper.Default.GetStringKey<string>(ReqUasSub);
                                if (!redisUasToken.Equals(ReqUasToken))
                                    message = "UasToken验证失败:redis中的key与传递的key不相等";
                            }
                        }
                        if (string.IsNullOrEmpty(message))
                        {
                            var rsa = new RSAHelper(RSAType.RSA2);
                            var decrypt = rsa.Decrypt(token.ReqUserToken);
                            if (string.IsNullOrEmpty(decrypt) || decrypt.Length != 36) message = "错误的用户令牌";
                            else if (decrypt != token.ReqUserId) message = "用户信息错误,请重新登录";
                        }
                    }
                    catch (Exception ex)
                    {
                        message = Util.ExceptionMessage(ex);
                    }
                    if (string.IsNullOrEmpty(message)) return;
                    context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.businessfail, Message = message }, setting);
                }
            }
            catch (Exception ex)
            {
                var message = Util.ExceptionMessage(ex);
                context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.businessfail, Message = message }, setting);
            }
        }
        private UserTokenModel getUserToken(StringValues ReqUserToken, StringValues ReqUserId, StringValues ReqDateExpire)
        {
            UserTokenModel token = null;
            if (ReqUserToken.Count > 0 && ReqUserId.Count > 0)
            {
                token = new UserTokenModel
                {
                    ReqUserId = ReqUserId[0],
                    ReqUserToken = ReqUserToken[0],
                    ReqDateExpire = ReqDateExpire.Count > 0 ? Util.ParseLong(ReqDateExpire[0]) : 0
                };
            }
            //Log.Log4netHelper.Error(this, JsonHelper.ObjectToJson(token));
            return token;
        }
    }
}