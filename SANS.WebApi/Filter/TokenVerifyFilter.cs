using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApi.Filter
{
    /// <summary>
    /// 用户令牌验证
    /// </summary>
    public class TokenVerifyFilter : ActionFilterAttribute
    {
    }
}
