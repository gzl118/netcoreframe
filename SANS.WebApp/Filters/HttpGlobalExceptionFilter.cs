using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SANS.WebApp.Data;
using SANS.WebApp.Models;
using SANS.Common;
using Newtonsoft.Json;
using SANS.Log;

namespace SANS.WebApp.Filters
{
    /// <summary>
    /// 全局异常处理过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        public HttpGlobalExceptionFilter(
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }
        public void OnException(ExceptionContext context)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                return;
            }
            JsonSerializerSettings setting = Util.GetJsonSetting();
            //记录错误日志
            Log4netHelper.Error(typeof(HttpGlobalExceptionFilter), context.Exception);
            //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.failure, Message = "服务器出现故障啦,请联系管理员查看错误日志!" }, setting);
            context.ExceptionHandled = true;
        }
    }
}
