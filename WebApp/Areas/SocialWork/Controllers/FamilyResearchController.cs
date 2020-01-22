using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Services;
using System;
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

            if(Id > 0)
            {
                
                return View(model);
            }

            return View(model);
        }
    }
}