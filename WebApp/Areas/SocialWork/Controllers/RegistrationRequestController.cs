using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Services;
using System;
using System.Linq;
using WebApp.Areas.SocialWork.Models;

namespace WebApp.Areas.SocialWork.Controllers
{
    [Area("SocialWork")]
    public class RegistrationRequestController : Controller
    {
        private const int numberOfRercordsToShow = 50;
        private readonly IRegistrationRequest registrationRequestRepository;

        public RegistrationRequestController(IRegistrationRequest _registrationRequestRepository)
        {
            registrationRequestRepository = _registrationRequestRepository;
        }

        [HttpGet]
        [Route("/SocialWork/RegistrationRequest")]
        public IActionResult Index()
        {
            RegistrationRequestViewModel model = new RegistrationRequestViewModel
            {
                RegistrationRequests = registrationRequestRepository.GetRegistrationRequestsWithMinimumData(numberOfRercordsToShow)
            };
            return View(model);
        }

        [HttpGet]
        [Route("/SocialWork/RegistrationRequest/ShowForm/{id?}")]
        public IActionResult ShowForm(int? Id)
        {
            RegistrationRequestViewModel model = new RegistrationRequestViewModel();
            model.LoadMaritalStatuses(registrationRequestRepository);
            model.LoadRelationships(registrationRequestRepository);
            model.LoadStatesOfMexico(registrationRequestRepository);
            model.LoadMunicipalitiesOfMexico(registrationRequestRepository);
            model.LoadRegistrationRequestStatuses(registrationRequestRepository);
            model.CreationDate = DateTime.Now;

            if (Id > 0) // item is stored in database already
            {
                var registration = registrationRequestRepository.GetRegistrationRequestById(Convert.ToInt32(Id));
                model.Comments = registration.Comments;
                model.CreationDate = registration.CreationDate;
                model.EconomicSituation = registration.EconomicSituation;
                model.FamilyComposition = registration.FamilyComposition;
                model.FamilyHealthStatus = registration.FamilyHealthStatus;
                model.FamilyInteraction = registration.FamilyInteraction;
                model.HowYouHearAboutUs = registration.HowYouHearAboutUs;
                model.Id = registration.Id;
                model.Minor = registration.Minor;
                model.MinorId = registration.MinorId;
                model.Reasons = registration.Reasons;
                model.Requestor = registration.Requestor;
                model.RequestorId = registration.RequestorId;
                model.SituationsOfDomesticViolence = registration.SituationsOfDomesticViolence;
                model.RegistrationRequestStatus = registration.RegistrationRequestStatus;
                model.RegistrationRequestStatusId = registration.RegistrationRequestStatusId;
                model.LoadMunicipalitiesOfMexico(registrationRequestRepository);
                model.LoadRegistrationRequestStatuses(registrationRequestRepository);

                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrUpdateRegistrationRequest([FromForm]RegistrationRequestViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Model cannot be null or empty");
                }

                if (model.IsValid(ModelState) == false)
                {
                    model.LoadMaritalStatuses(registrationRequestRepository);
                    model.LoadRelationships(registrationRequestRepository);
                    model.LoadStatesOfMexico(registrationRequestRepository);                    
                    model.LoadMunicipalitiesOfMexico(registrationRequestRepository);
                    model.LoadRegistrationRequestStatuses(registrationRequestRepository);
                    model.CreationDate = DateTime.Now;

                    return View("ShowForm", model);
                }

                if (model.Id > 0) // update item
                {
                    registrationRequestRepository.Update(model);
                }
                else // add new item
                {
                    registrationRequestRepository.Add(model);
                }
            }
            catch (BusinessLogicException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.LoadMaritalStatuses(registrationRequestRepository);
                model.LoadRelationships(registrationRequestRepository);
                model.LoadStatesOfMexico(registrationRequestRepository);
                model.LoadMunicipalitiesOfMexico(registrationRequestRepository);
                model.LoadRegistrationRequestStatuses(registrationRequestRepository);

                return View("ShowForm", model);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetMunicipalitiesOfMexico(string nameOfState)
        {
            if (string.IsNullOrEmpty(nameOfState))
            {
                return null;
            }

            System.Collections.Generic.List<Naandi.Shared.Models.MunicipalitiesOfMexico> MunicipalitiesOfMexico = registrationRequestRepository.GetMunicipalitiesOfMexicoByStateOfMexicoName(nameOfState).ToList();

            return Json(MunicipalitiesOfMexico);
        }

        [HttpGet]
        [Route("/SocialWork/RegistrationRequest/SearchRegistrationRequestsByMinorName/{minorName}")]
        public IActionResult SearchRegistrationRequestsByMinorName(string minorName)
        {
            if (string.IsNullOrEmpty(minorName))
            {
                return null;
            }

            RegistrationRequestViewModel model = new RegistrationRequestViewModel();

            if (string.Equals(minorName, "_all_", StringComparison.OrdinalIgnoreCase))
            {
                model.RegistrationRequests = registrationRequestRepository.GetRegistrationRequestsWithMinimumData(numberOfRercordsToShow);
            }
            else
            {
                model.RegistrationRequests = registrationRequestRepository.GetRegistrationRequestsByMinorName(minorName);
            }

            return PartialView("_RegistrationRequestTable", model);
        }

        [HttpPost]
        [Produces("application/json")]
        [Route("/SocialWork/RegistrationRequest/Delete")]
        public IActionResult Delete([FromForm]int Id)
        {
            if (Id > 0)
            {
                registrationRequestRepository.DeleteById(Id);
            }

            return StatusCode(StatusCodes.Status200OK, "Item deleted");
        }
    }
}