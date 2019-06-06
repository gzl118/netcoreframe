using SANS.Common;
using SANS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Filters
{
    public class ApiResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            JsonSerializerSettings setting = Util.GetJsonSetting();
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult.Value == null)
                {
                    context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.failure, Message = "数据为空！" }, setting);
                }
                else
                {
                    if (objectResult.Value is string)
                    {
                        context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.success, Data = objectResult.Value.ToString() }, setting);
                    }
                    else
                    {
                        context.Result = new JsonResult(objectResult.Value, setting);
                    }
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.fail404, Message = "未找到资源" }, setting);
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new JsonResult(new ApiReponseModel<string>() { Code = ApiResponseCode.success, Data = (context.Result as ContentResult).Content }, setting);
            }
            base.OnResultExecuting(context);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }

}
