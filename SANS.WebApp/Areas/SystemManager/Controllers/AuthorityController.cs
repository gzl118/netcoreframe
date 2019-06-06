using DInjectionProvider;
using Microsoft.AspNetCore.Mvc;
using SANS.WebApp.Controllers;

namespace SANS.WebApp.Areas.SystemManager.Controllers
{
    [Area("System")]
    public class AuthorityController : BaseController
    {
        private readonly WholeInjection injection;
        public AuthorityController(WholeInjection injection)
        {
            this.injection = injection;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}