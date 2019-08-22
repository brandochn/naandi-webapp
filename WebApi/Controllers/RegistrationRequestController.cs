using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        [Route("GetRegistrationRequests")]
        public IEnumerable<RegistrationRequest> GetRegistrationRequests()
        {
            return registrationRequestRepository.GetRegistrationRequests(numberOfRercordsToShow).ToList();
        }

        [HttpGet]
        [Route("GetStatesOfMexico")]
        public IEnumerable<StatesOfMexico> GetStatesOfMexico()
        {
            return registrationRequestRepository.GetStatesOfMexico().ToList();
        }

        [HttpGet]
        [Route("GetRegistrationRequestsByMinorName/{name}")]
        public IActionResult GetRegistrationRequestsByMinorName(string name)
        {
            var registrationRequests = registrationRequestRepository.GetRegistrationRequestsByMinorName(name);

            if(registrationRequests == null)
            {
                return NotFound("No record found");                
            }
            
            return Ok(registrationRequests.ToList());
        }
    }
}