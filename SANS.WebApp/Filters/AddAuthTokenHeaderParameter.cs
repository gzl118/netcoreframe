using SANS.WebApp.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Filters
{
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;
            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a.GetType().Equals(typeof(AnonymousFilter)));
                //非匿名的方法,链接中添加accesstoken值
                if (!isAnonymous)
                {
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "ReqUserToken",
                        In = "query",//query header body path formData
                        Type = "string",
                        Required = false //是否必选
                    });
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "ReqUserId",
                        In = "query",//query header body path formData
                        Type = "int",
                        Required = false //是否必选
                    });
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "ReqDateExpire",
                        In = "query",//query header body path formData
                        Type = "long",
                        Required = false //是否必选
                    });
                }
            }
        }
    }
}
