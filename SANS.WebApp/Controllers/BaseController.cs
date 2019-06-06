using Microsoft.AspNetCore.Mvc;
using SANS.WebApp.Filters;

namespace SANS.WebApp.Controllers
{
    [VerificationLogin]
    public class BaseController : Controller
    {

    }
}
