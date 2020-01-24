using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using Microsoft.Extensions.Logging;
using Naandi.Shared.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyResearchController : ControllerBase
    {
        private readonly IFamilyResearch familyResearchRepository;
        private readonly ILogger<FamilyResearchController> logger;
        public FamilyResearchController(IFamilyResearch _familyResearchRepository, ILogger<FamilyResearchController> _logger)
        {
            familyResearchRepository = _familyResearchRepository;
            logger = _logger;
        }

        [HttpPost]
        [Route("AddFamilyResearch")]
        public IActionResult AddFamilyResearch([FromBody]FamilyResearch familyResearch)
        {
            if (familyResearch == null)
            {
                return BadRequest("FamilyResearch cannot be null or empty");
            }

            if (familyResearch.LegalGuardian == null)
            {
                return BadRequest("LegalGuardian cannot be null or empty");
            }

            if (familyResearch.LegalGuardian.Address == null)
            {
                return BadRequest("Address cannot be null or empty");
            }

            if (familyResearch.Minor == null)
            {
                return BadRequest("Minor cannot be null or empty");
            }

            try
            {
                familyResearchRepository.Add(familyResearch);
            }
            catch(BusinessLogicException ble)
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

        [HttpGet]
        [Route("GetHomeAcquisitions")]
        public IEnumerable<HomeAcquisition> GetHomeAcquisitions()
        {
            return familyResearchRepository.GetHomeAcquisitions().ToList();
        }

        [HttpGet]
        [Route("GetTypesOfHouses")]
        public IEnumerable<TypesOfHouses> GetTypesOfHouses()
        {
            return familyResearchRepository.GetTypesOfHouses().ToList();
        }

        [HttpGet]
        [Route("GetTipoDeMobiliarios")]
        public IEnumerable<TipoDeMobiliario> GetTipoDeMobiliarios()
        {
            return familyResearchRepository.GetTipoDeMobiliarios().ToList();
        }

        [HttpGet]
        [Route("GetTypeOfDistricts")]
        public IEnumerable<TypeOfDistrict> GetTypeOfDistricts()
        {
            return familyResearchRepository.GetTypeOfDistricts().ToList();
        }

        [HttpGet]
        [Route("GetPatrimonies")]
        public IEnumerable<Patrimony> GetPatrimonies()
        {
            return familyResearchRepository.GetPatrimonies().ToList();
        }

        [HttpGet]
        [Route("GetFoods")]
        public IEnumerable<Food> GetFoods()
        {
            return familyResearchRepository.GetFoods().ToList();
        }

        [HttpGet]
        [Route("GetFrequencies")]
        public IEnumerable<Frequency> GetFrequencies()
        {
            return familyResearchRepository.GetFrequencies().ToList();
        }

        [HttpGet]
        [Route("GetFamilyResearchById/{Id}")]
        public IActionResult GetFamilyResearchById(int Id)
        {
            var familyResearch = familyResearchRepository.GetFamilyResearchById(Id);

            if (familyResearch == null)
            {
                return NotFound("No record found");
            }

            return Ok(familyResearch);
        }

        [HttpGet]
        [Route("GetFamilyResearches")]
        public IEnumerable<FamilyResearch> GetFamilyResearches()
        {
            return familyResearchRepository.GetFamilyResearches().ToList();
        }
    }
}