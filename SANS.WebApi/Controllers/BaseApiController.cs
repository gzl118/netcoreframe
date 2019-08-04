using Microsoft.AspNetCore.Mvc;
using SANS.WebApi.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApi.Controllers
{
    [ApiController]
    [TypeFilter(typeof(LogFilter))]
    public class BaseApiController : ControllerBase
    {
    }
}
