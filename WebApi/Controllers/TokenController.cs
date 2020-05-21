using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IJwt jwtRepository;
        private readonly ILogger<TokenController> logger;
        private readonly IUser userResearchRepository;

        public TokenController(IJwt _jwtRepository, ILogger<TokenController> _logger, IUser _userResearchRepository)
        {
            jwtRepository = _jwtRepository;
            logger = _logger;
            userResearchRepository = _userResearchRepository;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(string username, string password)
        {
            try
            {
                User user = new User()
                {
                    UserName = username,
                    Password = password
                };

                if (userResearchRepository.ValidateLogin(user))
                {
                    var claims = userResearchRepository.GetClaimsByByUserName(user.UserName).ToList();

                    return new OkObjectResult(jwtRepository.GenerateSecurityToken(claims));
                }
                else
                {
                    string errorMessage = "Invalid UserName or Password";
                    logger.LogWarning(errorMessage);
                    return StatusCode(StatusCodes.Status401Unauthorized, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNHANDLED_EXCEPTION_MESSAGE);
        }
    }
}