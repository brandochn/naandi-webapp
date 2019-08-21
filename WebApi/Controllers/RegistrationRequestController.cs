using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Models;
using Naandi.Shared.Services;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationRequestController : ControllerBase
    {
        private readonly IRegistrationRequest registrationRequestRepository;
        private const int numberOfRercordsToShow = 5000;

        public RegistrationRequestController(IRegistrationRequest _registrationRequestRepository)
        {
            registrationRequestRepository = _registrationRequestRepository;
        }

        [HttpGet(Name = "GetRegistrationRequests")]
        public IEnumerable<RegistrationRequest> Get()
        {
            return registrationRequestRepository.GetRegistrationRequests(numberOfRercordsToShow);
        }
    }
}