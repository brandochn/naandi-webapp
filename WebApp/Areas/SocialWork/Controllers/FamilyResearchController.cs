using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Services;
using System;
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
    }
}