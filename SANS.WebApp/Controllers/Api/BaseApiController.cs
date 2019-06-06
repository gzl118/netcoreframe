using SANS.WebApp.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Controllers.Api
{
    /// <summary>
    /// API接口基类
    /// </summary>
    [ApiController]
    [TokenVerifyFilter]
    [ApiResultFilter]
    public class BaseApiController : ControllerBase
    {

    }
}
