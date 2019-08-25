using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    public class LoginController : Controller
    {
        public List<UserModel> Users => new List<UserModel>() {
            new UserModel { UserName = "gzl", UserPwd = "123" },
            new UserModel{ UserName = "aaa", UserPwd = "12345" }
        };
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Defude()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserModel user, string returnUrl = null)
        {
            const string badUserNameOrPasswordMessage = "Username or password is incorrect.";
            if (user == null)
            {
                return BadRequest(badUserNameOrPasswordMessage);
            }
            var lookupUser = Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (lookupUser?.UserPwd != user.UserPwd)
            {
                return BadRequest(badUserNameOrPasswordMessage);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, lookupUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (returnUrl == null)
            {
                returnUrl = TempData["returnUrl"]?.ToString();
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}