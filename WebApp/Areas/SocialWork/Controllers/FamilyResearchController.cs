using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Areas.SocialWork.Models;
using WebApp.ExtensionMethods;

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
            if (ModelState.IsValid == true)
            {
                model.FullName = model.FullName?.Trim();
                SessionState.UserSession.AddItemInDataCollection<FamilyMembersDetails>(Constants.FamilyResearch_FamilyMembers_Table, model);
            }

            var familyMembersDetails = new FamilyMembersViewModel();
            familyMembersDetails.LoadMaritalStatuses(familyResearchRepository);
            familyMembersDetails.LoadRelationships(familyResearchRepository);
            familyMembersDetails.Age = model?.Age;
            familyMembersDetails.CurrentOccupation = model?.CurrentOccupation;
            familyMembersDetails.Education = model?.Education;
            familyMembersDetails.FullName = model?.FullName;
            familyMembersDetails.MaritalStatusId = model?.MaritalStatusId ?? 0;
            familyMembersDetails.RelationshipId = model?.RelationshipId ?? 0;

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

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetBenefitsProvidedForm")]
        public IActionResult GetBenefitsProvidedForm()
        {
            var model = new BenefitsProvidedViewModel();

            return PartialView("_BenefitsProvidedForm", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/AddItemInBenefitsProvidedTable")]
        public IActionResult AddItemInBenefitsProvidedTable([FromBody]BenefitsProvidedDetails model)
        {
            BenefitsProvidedViewModel benefitsProvidedViewModel = new BenefitsProvidedViewModel()
            {
                ApoyoRecibido = model?.ApoyoRecibido,
                Institucion = model?.Institucion,
                Monto = model?.Monto,
                Periodo = model?.Periodo
            };

            if (ModelState.IsValid == true)
            {
                benefitsProvidedViewModel.Key = string.Empty.GetUniqueKey();
                SessionState.UserSession.AddItemInDataCollection<BenefitsProvidedViewModel>(Constants.FamilyResearch_BenefitsProvided_Table, benefitsProvidedViewModel);
            }

            return PartialView("_BenefitsProvidedForm", benefitsProvidedViewModel);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetBenefitsProvidedTable")]
        public IActionResult GetBenefitsProvidedTable()
        {
            List<BenefitsProvidedViewModel> model = SessionState.UserSession.GetDataCollection<List<BenefitsProvidedViewModel>>(Constants.FamilyResearch_BenefitsProvided_Table);

            return PartialView("_BenefitsProvidedTable", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/RemoveItemInBenefitsProvidedTable")]
        public IActionResult RemoveItemInBenefitsProvidedTable(string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return BadRequest("key cannot be null or empty");
            }

            var table = SessionState.UserSession.GetDataCollection<List<BenefitsProvidedViewModel>>(Constants.FamilyResearch_BenefitsProvided_Table);

            if (table != null)
            {
                var index = table.FindIndex(r => string.Equals(r.Key, key, StringComparison.OrdinalIgnoreCase));

                if (index >= 0 && index < table.Count)
                {
                    SessionState.UserSession.RemoveItemInDataCollection<BenefitsProvidedViewModel>(Constants.FamilyResearch_BenefitsProvided_Table, index);
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetIngresosMensualesForm")]
        public IActionResult GetIngresosMensualesForm()
        {
            var model = new IngresosMensualesViewModel();
            model.LoadMovimientoList(familyResearchRepository);

            return PartialView("_IngresosMensualesForm", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/AddItemInIngresosMensualesTable")]
        public IActionResult AddItemInIngresosMensualesTable([FromBody]IngresosEgresosMensualesMovimientoRelation model)
        {
            IngresosMensualesViewModel ingresosMensualesViewModel = new IngresosMensualesViewModel()
            {
                Monto = model?.Monto,
                MovimientoId = model?.MovimientoId
            };

            ingresosMensualesViewModel.LoadMovimientoList(familyResearchRepository);

            if (ModelState.IsValid == true)
            {
                ingresosMensualesViewModel.Key = string.Empty.GetUniqueKey();
                SessionState.UserSession.AddItemInDataCollection<IngresosMensualesViewModel>(Constants.FamilyResearch_IngresosMensuales_Table, ingresosMensualesViewModel);
            }

            return PartialView("_IngresosMensualesForm", ingresosMensualesViewModel);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetIngresosMensualesTable")]
        public IActionResult GetIngresosMensualesTable()
        {
            IngresosMensualesViewModel ingresosMensualesViewModel = new IngresosMensualesViewModel();
            List<IngresosMensualesViewModel> table = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_IngresosMensuales_Table);
            if (table != null)
            {
                ingresosMensualesViewModel.LoadMovimientoList(familyResearchRepository);

                foreach (var iter in table)
                {
                    iter.Movimiento = ingresosMensualesViewModel.MovimientoList.Where(m => m.Id == iter.MovimientoId).FirstOrDefault();
                }
            }

            return PartialView("_IngresosMensualesTable", table);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/RemoveItemInIngresosMensualesTable")]
        public IActionResult RemoveItemInIngresosMensualesTable(string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return BadRequest("key cannot be null or empty");
            }

            var table = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_IngresosMensuales_Table);

            if (table != null)
            {
                var index = table.FindIndex(r => string.Equals(r.Key, key, StringComparison.OrdinalIgnoreCase));

                if (index >= 0 && index < table.Count)
                {
                    SessionState.UserSession.RemoveItemInDataCollection<IngresosMensualesViewModel>(Constants.FamilyResearch_IngresosMensuales_Table, index);
                }
            }

            return Ok();
        }
    }
}