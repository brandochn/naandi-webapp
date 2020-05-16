using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser userResearchRepository;

        public AccountController(IUser _userResearchRepository)
        {
            userResearchRepository = _userResearchRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Account/Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            User user = new User()
            {
                UserName = userName,
                Password = password
            };

            if (userResearchRepository.ValidateLogin(user))
            {
                var claims = userResearchRepository.GetClaimsByByUserName(user.UserName);

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "CookieAuth", ClaimTypes.Name, ClaimTypes.Role)));

                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Route("/Account/AccessDenied/{returnUrl?}")]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}