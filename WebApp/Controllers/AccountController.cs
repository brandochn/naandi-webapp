using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.SessionState;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser userResearchRepository;
        private readonly ILogger<AccountController> logger;

        public AccountController(IUser _userResearchRepository, ILogger<AccountController> _logger)
        {
            userResearchRepository = _userResearchRepository;
            logger = _logger;
        }

        [HttpGet]
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

            try
            {
                if (userResearchRepository.ValidateLogin(user))
                {
                    var claims = userResearchRepository.GetClaimsByByUserName(user.UserName);
                    // Sign in to WebApp
                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "CookieAuth", ClaimTypes.Name, ClaimTypes.Role)));

                    // Sign in to WebApi
                    var token = userResearchRepository.CreateToken(user.UserName, user.Password);

                    token = token.Replace("\"", "");

                    UserSession.SetToken(token);

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    string error = "Invalid UserName or Password";
                    ModelState.AddModelError("", error);
                    logger.LogWarning(error);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
            }

            return View();
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