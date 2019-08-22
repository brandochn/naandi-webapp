using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Route("GetRegistrationRequestById/{Id}")]
        public IActionResult GetRegistrationRequestById(int Id)
        {
            var registrationRequest = registrationRequestRepository.GetRegistrationRequestById(Id);

            if(registrationRequest == null)
            {
                return NotFound("No record found");
            }

            return Ok(registrationRequest);
        }

        [HttpGet]
        [Route("GetMunicipalitiesOfMexicoByStateOfMexicoName/{nameOfState}")]
        public IActionResult GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState)
        {
            if (nameOfState == null)
            {
                return BadRequest("nameOfState cannot be null or empty");
            }

            var municipalities = registrationRequestRepository.GetMunicipalitiesOfMexicoByStateOfMexicoName(nameOfState);

            if (municipalities == null)
            {
                return NotFound("No record found");
            }

            return Ok(municipalities.ToList());

        }

        [HttpGet]
        [Route("GetMaritalStatuses")]
        public IEnumerable<MaritalStatus> GetMaritalStatuses()
        {
            return registrationRequestRepository.GetMaritalStatuses().ToList();
        }

        [HttpGet]
        [Route("GetRelationships")]
        public IEnumerable<Relationship> GetRelationships()
        {
            return registrationRequestRepository.GetRelationships().ToList();
        }

        [HttpGet]
        [Route("RegistrationRequestStatuses")]
        public IEnumerable<RegistrationRequestStatus> RegistrationRequestStatuses()
        {
            return registrationRequestRepository.RegistrationRequestStatuses().ToList();
        }

        [HttpPost]
        [Route("AddRegistrationRequest")]
        public IActionResult AddRegistrationRequest([FromBody]RegistrationRequest registrationRequest)
        {
            if (registrationRequest == null)
            {
                return BadRequest("registrationRequest cannot be null or empty");
            }

            try
            {
                registrationRequestRepository.Add(registrationRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNHANDLED_EXCEPTION_MESSAGE);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        [Route("UpdateRegistrationRequest")]
        public IActionResult UpdateRegistrationRequest([FromBody] RegistrationRequest registrationRequest)
        {
            if (registrationRequest == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "registrationRequest cannot be null or empty");
            }         

            try
            {
                registrationRequestRepository.Update(registrationRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNHANDLED_EXCEPTION_MESSAGE);
            }

            return Ok();
        }
    }
}