using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GZLController : BaseApiController
    {
        [HttpGet]
        public string GetName()
        {
            return "gzl";
        }
        [HttpPost]
        public string AddName([FromForm]string sName, [FromForm] string sAge)
        {

            return sName;
        }
    }
}
