using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SANS.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SANS.WebApi.Extension;

namespace SANS.WebApi.Filter
{
    public class LogFilter : ActionFilterAttribute
    {
        private string ActionArguments { get; set; }
        private readonly CustomConfiguration customConfiguration;
        public LogFilter(IOptionsSnapshot<CustomConfiguration> _customConfiguration)
        {
            customConfiguration = _customConfiguration.Value;
        }
        /// <summary>
        /// 请求体中的所有值
        /// </summary>
        private string RequestBody { get; set; }

        private Stopwatch Stopwatch { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!customConfiguration.IsLog)
                return;
            try
            {
                // 后续添加了获取请求的请求体，如果在实际项目中不需要删除即可
                long contentLen = context.HttpContext.Request.ContentLength == null ? 0 : context.HttpContext.Request.ContentLength.Value;
                if (contentLen > 0)
                {
                    // 读取请求体中所有内容
                    System.IO.Stream stream = context.HttpContext.Request.Body;
                    if (stream.CanSeek)
                    {
                        if (context.HttpContext.Request.Method == "POST")
                        {
                            stream.Position = 0;
                        }
                        byte[] buffer = new byte[contentLen];
                        stream.Read(buffer, 0, buffer.Length);
                        // 转化为字符串
                        RequestBody = System.Text.Encoding.UTF8.GetString(buffer);
                    }
                    //else
                    //{
                    //    var prams = "";
                    //    foreach (var item in context.HttpContext.Request.Form.Keys)
                    //    {
                    //        prams += item + ":" + context.HttpContext.Request.Form[item].ToString();
                    //    }
                    //    RequestBody = prams;
                    //}
                }

                ActionArguments = Newtonsoft.Json.JsonConvert.SerializeObject(context.ActionArguments);

                Stopwatch = new Stopwatch();
                Stopwatch.Start();
            }
            catch (Exception)
            {
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (!customConfiguration.IsLog)
                return;
            try
            {

                Stopwatch.Stop();
                StringBuilder builder = new StringBuilder();
                string url = context.HttpContext.Request.Host + context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                string method = context.HttpContext.Request.Method;

                string qs = ActionArguments;

                string controllerName = context.RouteData.Values["Controller"].ToString();//通过ActionContext类的RouteData属性获取Controller的名称：Home
                string actionName = context.RouteData.Values["Action"].ToString();

                dynamic result = context.Result.GetType().Name == "EmptyResult" ? new { Value = "无返回结果" } : context.Result as dynamic;

                var clientIP = context.HttpContext.GetClientUserIp();

                string res = "在返回结果前发生了异常";
                try
                {
                    if (result != null)
                    {
                        res = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
                    }
                }
                catch (System.Exception)
                {
                    res = "日志未获取到结果，返回的数据无法序列化";
                }
                builder.AppendLine($"");
                builder.AppendLine($"Controller：{controllerName},Action：{actionName}");
                builder.AppendLine($"客户端IP：{clientIP}");
                builder.AppendLine($"地址：{url}");
                builder.AppendLine($"方式：{method}");
                builder.AppendLine($"请求体：{RequestBody}");
                builder.AppendLine($"参数：{qs}");
                builder.AppendLine($"结果：{res}");
                builder.AppendLine($"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒（指控制器内对应方法执行完毕的时间）");
                Log4netHelper.Info(this, builder.ToString());
            }
            catch (Exception)
            {
            }
        }
    }
}
