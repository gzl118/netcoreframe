using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SANS.Common;
using SANS.WebApi.Filter;

namespace SANS.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseApiController
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ValuesController(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("[action]")]
        public User AddUser(User u)
        {
            return u;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<string> GetGZL()
        {
            var url = "http://localhost:63604/api/Values/AddUser";
            User m = new User();
            m.UserName = "gzl123";
            m.Oid = "12344";
            var client = httpClientFactory.CreateClient();
            var result = await client.PostAsJsonAsync(url, m);
            if (result.IsSuccessStatusCode)
            {
                var a = result.Content.ReadAsAsync<User>().Result;
            }
            return "失败";
        }
    }
    public class User
    {
        public string UserName { get; set; }
        public string Oid { get; set; }
        public int Age { get; set; }
    }
}
