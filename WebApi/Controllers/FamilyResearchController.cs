using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Models;
using Naandi.Shared.Services;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyResearchController : ControllerBase
    {
        private readonly IFamilyResearch familyResearchRepository;
        public FamilyResearchController(IFamilyResearch _familyResearchRepository)
        {
            familyResearchRepository = _familyResearchRepository;
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
            catch (Exception ex)
            {
                // TODO: save ex object in log error
                return StatusCode(StatusCodes.Status500InternalServerError, Constants.UNHANDLED_EXCEPTION_MESSAGE);
            }

            return Ok();
        }
    }
}