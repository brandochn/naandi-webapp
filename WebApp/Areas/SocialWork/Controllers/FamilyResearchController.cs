using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Areas.SocialWork.Models;

namespace WebApp.Areas.SocialWork.Controllers
{
    [Area("SocialWork")]
    public class FamilyResearchController : Controller
    {
        private readonly IFamilyResearch familyResearchRepository;
        public FamilyResearchController(IFamilyResearch _familyResearchRepository)
        {
            familyResearchRepository = _familyResearchRepository;
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch")]
        public IActionResult Index()
        {
            FamilyResearchViewModel model = new FamilyResearchViewModel
            {
                FamilyResearches = familyResearchRepository.GetFamilyResearches().ToList()
            };
            return View(model);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/ShowForm/{id?}")]
        public IActionResult ShowForm(int? Id)
        {
            FamilyResearchViewModel model = new FamilyResearchViewModel();
            model.LoadMaritalStatuses(familyResearchRepository);
            model.LoadRelationships(familyResearchRepository);
            model.LoadStatesOfMexico(familyResearchRepository);
            model.LoadMunicipalitiesOfMexico(familyResearchRepository);
            model.LoadHomeAcquisitionList(familyResearchRepository);
            model.LoadTipoDeMobiliarioList(familyResearchRepository);
            model.LoadTypeOfDistrictList(familyResearchRepository);
            model.LoadPatrimonyViewModelCollection(familyResearchRepository);
            model.LoadFoods(familyResearchRepository);
            model.LoadFrequencies(familyResearchRepository);
            model.LoadTypesOfHousesList(familyResearchRepository);
            model.VisitDate = DateTime.Now;
            model.VisitTime = DateTime.Now;

            if (Id > 0)
            {

                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrUpdateFamilyResearch([FromForm]FamilyResearchViewModel model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Model cannot be null or empty");
                }

                if (model.IsValid(ModelState) == false)
                {
                    model.LoadMaritalStatuses(familyResearchRepository);
                    model.LoadRelationships(familyResearchRepository);
                    model.LoadStatesOfMexico(familyResearchRepository);
                    model.LoadMunicipalitiesOfMexico(familyResearchRepository);
                    model.LoadHomeAcquisitionList(familyResearchRepository);
                    model.LoadTipoDeMobiliarioList(familyResearchRepository);
                    model.LoadTypeOfDistrictList(familyResearchRepository);
                    model.LoadPatrimonyViewModelCollection(familyResearchRepository);
                    model.LoadFoods(familyResearchRepository);
                    model.LoadFrequencies(familyResearchRepository);
                    model.LoadTypesOfHousesList(familyResearchRepository);

                    return View("ShowForm", model);
                }

                if (model.Id > 0) // update item
                {
                    familyResearchRepository.Update(model);
                }
                else // add new item
                {
                    familyResearchRepository.Add(model);
                }
            }
            catch (BusinessLogicException ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.LoadMaritalStatuses(familyResearchRepository);
                model.LoadRelationships(familyResearchRepository);
                model.LoadStatesOfMexico(familyResearchRepository);
                model.LoadMunicipalitiesOfMexico(familyResearchRepository);
                model.LoadHomeAcquisitionList(familyResearchRepository);
                model.LoadTipoDeMobiliarioList(familyResearchRepository);
                model.LoadTypeOfDistrictList(familyResearchRepository);
                model.LoadPatrimonyViewModelCollection(familyResearchRepository);
                model.LoadFoods(familyResearchRepository);
                model.LoadFrequencies(familyResearchRepository);
                model.LoadTypesOfHousesList(familyResearchRepository);

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

            System.Collections.Generic.List<Naandi.Shared.Models.MunicipalitiesOfMexico> MunicipalitiesOfMexico = familyResearchRepository.GetMunicipalitiesOfMexicoByStateOfMexicoName(nameOfState).ToList();

            return Json(MunicipalitiesOfMexico);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetFamilyMembersForm")]
        public IActionResult GetFamilyMembersForm()
        {
            var model = new FamilyMembersViewModel();
            model.LoadMaritalStatuses(familyResearchRepository);
            model.LoadRelationships(familyResearchRepository);

            return PartialView("_FamilyMembersForm", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/AddItemInFamilyMembersTable")]
        public IActionResult AddItemInFamilyMembersTable([FromBody]FamilyMembersDetails model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid == true)
            {
                model.FullName = model.FullName?.Trim();
                SessionState.UserSession.AddItemInDataCollection<FamilyMembersDetails>(Constants.FamilyResearch_FamilyMembers_Table, model);              
            }

            var familyMembersDetails = new FamilyMembersViewModel();
            familyMembersDetails.LoadMaritalStatuses(familyResearchRepository);
            familyMembersDetails.LoadRelationships(familyResearchRepository);
            familyMembersDetails.Age = model.Age;
            familyMembersDetails.CurrentOccupation = model.CurrentOccupation;
            familyMembersDetails.Education = model.Education;
            familyMembersDetails.FullName = model.FullName;
            familyMembersDetails.MaritalStatusId = model.MaritalStatusId;
            familyMembersDetails.RelationshipId = model.RelationshipId;

            return PartialView("_FamilyMembersForm", familyMembersDetails);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetFamilyMembersTable")]
        public IActionResult GetFamilyMembersTable()
        {
            var model = new FamilyResearchViewModel();
            model.FamilyMembers = new Naandi.Shared.Models.FamilyMembers();
            var details = SessionState.UserSession.GetDataCollection<List<FamilyMembersDetails>>(Constants.FamilyResearch_FamilyMembers_Table);

            if (details != null)
            {
                model.LoadMaritalStatuses(familyResearchRepository);
                model.LoadRelationships(familyResearchRepository);

                foreach (var d in details)
                {
                    d.MaritalStatus = model.MaritalStatusList.Where(ms => ms.Id == d.MaritalStatusId).FirstOrDefault();
                    d.Relationship = model.RelationshipList.Where(r => r.Id == d.RelationshipId).FirstOrDefault();
                }

                model.FamilyMembers.FamilyMembersDetails = details?.ToArray();
            }

            return PartialView("_FamilyMembersTable", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/RemoveItemInFamilyMembersTable")]
        public IActionResult RemoveItemInFamilyMembersTable(string name)
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                return BadRequest("name cannot be null or empty");
            }
            
            var details = SessionState.UserSession.GetDataCollection<List<FamilyMembersDetails>>(Constants.FamilyResearch_FamilyMembers_Table);

            if (details != null)
            {
                var index = details.FindIndex(d => string.Equals(d.FullName, name, StringComparison.OrdinalIgnoreCase));

                if (index >= 0 && index < details.Count)
                {
                    SessionState.UserSession.RemoveItemInDataCollection<FamilyMembersDetails>(Constants.FamilyResearch_FamilyMembers_Table, index);
                }
            }

            return Ok();
        }
    }
}