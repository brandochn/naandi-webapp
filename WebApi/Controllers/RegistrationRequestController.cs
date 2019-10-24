using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Naandi.Shared.Exceptions;
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
        private readonly ILogger<RegistrationRequestController> logger;

        public RegistrationRequestController(IRegistrationRequest _registrationRequestRepository, ILogger<RegistrationRequestController> _logger)
        {
            registrationRequestRepository = _registrationRequestRepository;
            logger = _logger;
        }

        [HttpGet]
        [Route("GetRegistrationRequests")]
        public IEnumerable<RegistrationRequest> GetRegistrationRequests()
        {
            return registrationRequestRepository.GetRegistrationRequests().ToList();
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

            if (registrationRequests == null)
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

            if (registrationRequest == null)
            {
                return NotFound("No record found");
            }

            return Ok(registrationRequest);
        }

        [HttpGet]
        [Route("GetMunicipalitiesOfMexicoByStateOfMexicoName/{nameOfState}")]
        public IActionResult GetMunicipalitiesOfMexicoByStateOfMexicoName(string nameOfState)
        {
            if (string.IsNullOrEmpty(nameOfState))
            {
                return BadRequest("nameOfState cannot be null or empty");
            }

            var municipalities = registrationRequestRepository.GetMunicipalitiesOfMexicoByStateOfMexicoName(nameOfState);

            if (municipalities == null || municipalities.Count() == 0)
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
            catch (BusinessLogicException ble)
            {
                logger.LogWarning(ble.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ble.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNHANDLED_EXCEPTION_MESSAGE);
            }

            return Ok();
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
            catch (BusinessLogicException ble)
            {
                logger.LogWarning(ble.Message);
                return StatusCode(StatusCodes.Status400BadRequest, ble.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null);
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNHANDLED_EXCEPTION_MESSAGE);
            }

            return Ok();
        }
    }
}