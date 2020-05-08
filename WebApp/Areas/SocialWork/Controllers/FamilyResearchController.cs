using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Naandi.Shared.Exceptions;
using Naandi.Shared.Models;
using Naandi.Shared.Services;
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
                FamilyResearches = familyResearchRepository.GetFamilyResearches()?.ToList()
            };

            ClearSessionForTablesObject();

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
            model.SetInitialPatrimonyViewModelCollection(familyResearchRepository);
            model.LoadFoods(familyResearchRepository);
            model.LoadFrequencies(familyResearchRepository);
            model.LoadTypesOfHousesList(familyResearchRepository);
            model.VisitDate = DateTime.Now;
            model.FormVisitTime = DateTime.Now.ToShortTimeString();

            if (Id > 0)
            {
                FamilyResearch familyResearch = familyResearchRepository.GetFamilyResearchById((int)Id);
                model.Id = familyResearch.Id;
                model.CaseStudyConclusion = familyResearch.CaseStudyConclusion;
                model.District = familyResearch.District;
                model.DistrictId = familyResearch.DistrictId;
                model.EconomicSituationId = familyResearch.EconomicSituationId;
                model.Family = familyResearch.Family;
                model.FamilyDiagnostic = familyResearch.FamilyDiagnostic;
                model.FamilyExpectations = familyResearch.FamilyExpectations;
                model.FamilyHealth = familyResearch.FamilyHealth;
                model.FamilyHealthId = familyResearch.FamilyHealthId;
                model.FamilyNutrition = familyResearch.FamilyNutrition;
                model.FamilyNutritionId = familyResearch.FamilyNutritionId;
                model.LegalGuardian = familyResearch.LegalGuardian;
                model.LegalGuardianId = familyResearch.LegalGuardianId;
                model.Minor = familyResearch.Minor;
                model.MinorId = familyResearch.MinorId;
                model.Minor.FormalEducation = familyResearch.Minor.FormalEducation;
                model.Minor.FormalEducationId = familyResearch.Minor.FormalEducationId;
                model.PreviousFoundation = familyResearch.PreviousFoundation;
                model.PreviousFoundationId = familyResearch.PreviousFoundationId;
                model.ProblemsIdentified = familyResearch.ProblemsIdentified;
                model.Recommendations = familyResearch.Recommendations;
                model.RedesDeApoyoFamiliares = familyResearch.RedesDeApoyoFamiliares;
                model.RequestReasons = familyResearch.RequestReasons;
                model.SituationsOfDomesticViolence = familyResearch.SituationsOfDomesticViolence;
                model.Sketch = familyResearch.Sketch;
                model.SocioEconomicStudy = familyResearch.SocioEconomicStudy;
                model.SocioEconomicStudyId = familyResearch.SocioEconomicStudyId;
                model.VisualSupports = familyResearch.VisualSupports;
                model.LoadMunicipalitiesOfMexico(familyResearchRepository);
                model.FormVisitTime = familyResearch.VisitTime.ToShortTimeString();
                model.VisitDate = familyResearch.VisitDate;

                model.LoadFamilyNutritionFoodRelation(familyResearch.FamilyNutrition);
                model.LoadPatrimonyViewModelCollection(familyResearch.EconomicSituation);
                model.FamilyMembers = familyResearch.FamilyMembers;
                model.FamilyMembersId = familyResearch.FamilyMembersId;
                model.SetFamilyMembersInSession(familyResearch.FamilyMembers);
                model.BenefitsProvidedList = model.ConvertBenefitsProvidedToBenefitsProvidedViewModel(familyResearch.BenefitsProvided?.BenefitsProvidedDetails);
                model.BenefitsProvidedId = familyResearch.BenefitsProvidedId;
                model.IngresosEgresosMensuales = familyResearch.IngresosEgresosMensuales;
                model.IngresosMensualesList = model.ConvertIngresosEgresosMensualesMovimientoRelationToIngresosMensualesViewModel(familyResearch.IngresosEgresosMensuales?.IngresosEgresosMensualesMovimientoRelation);
                model.IngresosEgresosMensualesId = familyResearch.IngresosEgresosMensualesId;
                model.EgresosMensualesList = model.ConvertIngresosEgresosMensualesMovimientoRelationToEgresosMensualesViewModel(familyResearch.IngresosEgresosMensuales?.IngresosEgresosMensualesMovimientoRelation);

                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddOrUpdateFamilyResearch([FromForm] FamilyResearchViewModel model)
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
                    model.SetInitialPatrimonyViewModelCollection(familyResearchRepository);
                    model.LoadFoods(familyResearchRepository);
                    model.LoadFrequencies(familyResearchRepository);
                    model.LoadTypesOfHousesList(familyResearchRepository);

                    return View("ShowForm", model);
                }

                model.VisitTime = DateTime.Parse(model.FormVisitTime);
                model.GetEconomicSituationPatrimonyRelationFromViewModel(familyResearchRepository);
                model.GetFamilyNutritionFoodRelationFromViewModel(familyResearchRepository);
                model.GetBenefitsProvidedFromSession();
                model.GetIngresosMensualesFromSession();
                model.GetFamilyMembersFromSession();

                if (model.Id > 0) // update item
                {
                    if (model.LegalGuardianId > 0)
                    {
                        model.LegalGuardian.Id = Convert.ToInt32(model.LegalGuardianId);
                    }

                    if (model.MinorId > 0)
                    {
                        model.Minor.Id = Convert.ToInt32(model.MinorId);
                    }

                    if (model.PreviousFoundationId > 0)
                    {
                        model.PreviousFoundation.Id = Convert.ToInt32(model.PreviousFoundationId);
                    }

                    if (model.FamilyHealthId > 0)
                    {
                        model.FamilyHealth.Id = Convert.ToInt32(model.FamilyHealthId);
                    }

                    if (model.SocioEconomicStudyId > 0)
                    {
                        model.SocioEconomicStudy.Id = Convert.ToInt32(model.SocioEconomicStudyId);
                    }

                    if (model.IngresosEgresosMensualesId > 0)
                    {
                        model.IngresosEgresosMensuales.Id = Convert.ToInt32(model.IngresosEgresosMensualesId);
                    }

                    if (model.DistrictId > 0)
                    {
                        model.District.Id = Convert.ToInt32(model.DistrictId);
                    }

                    if (model.Minor.FormalEducationId > 0)
                    {
                        model.Minor.FormalEducation.Id = Convert.ToInt32(model.Minor.FormalEducationId);
                    }

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
                model.SetInitialPatrimonyViewModelCollection(familyResearchRepository);
                model.LoadFoods(familyResearchRepository);
                model.LoadFrequencies(familyResearchRepository);
                model.LoadTypesOfHousesList(familyResearchRepository);

                return View("ShowForm", model);
            }
            catch (Exception)
            {
                throw;
            }

            ClearSessionForTablesObject();

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
        public IActionResult AddItemInFamilyMembersTable([FromBody] FamilyMembersDetails model)
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
        public IActionResult AddItemInBenefitsProvidedTable([FromBody] BenefitsProvidedDetails model)
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
            ViewBag.HasErrorMessage = false;

            return PartialView("_IngresosMensualesForm", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/AddItemInIngresosMensualesTable")]
        public IActionResult AddItemInIngresosMensualesTable([FromBody] IngresosEgresosMensualesMovimientoRelation model)
        {
            IngresosMensualesViewModel ingresosMensualesViewModel = new IngresosMensualesViewModel()
            {
                Monto = model?.Monto,
                MovimientoId = model?.MovimientoId
            };

            ViewBag.HasErrorMessage = false;

            ingresosMensualesViewModel.LoadMovimientoList(familyResearchRepository);

            if (ModelState.IsValid == true)
            {
                bool IsItemValid = true;
                List<IngresosMensualesViewModel> table = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_IngresosMensuales_Table);
                if (table != null)
                {
                    foreach (var iter in table)
                    {
                        if (iter.MovimientoId == model.MovimientoId)
                        {
                            ViewBag.ErrorMessage = "No puede duplicar los ingresos";
                            ViewBag.HasErrorMessage = true;
                            IsItemValid = false;
                            break;
                        }
                    }
                }

                if (IsItemValid == true)
                {
                    ingresosMensualesViewModel.Key = string.Empty.GetUniqueKey();
                    SessionState.UserSession.AddItemInDataCollection<IngresosMensualesViewModel>(Constants.FamilyResearch_IngresosMensuales_Table, ingresosMensualesViewModel);
                }
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

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetEgresosMensualesForm")]
        public IActionResult GetEgresosMensualesForm()
        {
            var model = new EgresosMensualesViewModel();
            model.LoadMovimientoList(familyResearchRepository);
            ViewBag.HasErrorMessage = false;

            return PartialView("_EgresosMensualesForm", model);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/AddItemInEgresosMensualesTable")]
        public IActionResult AddItemInEgresosMensualesTable([FromBody] IngresosEgresosMensualesMovimientoRelation model)
        {
            EgresosMensualesViewModel egresosMensualesViewModel = new EgresosMensualesViewModel()
            {
                Monto = model?.Monto,
                MovimientoId = model?.MovimientoId
            };

            ViewBag.HasErrorMessage = false;

            egresosMensualesViewModel.LoadMovimientoList(familyResearchRepository);

            if (ModelState.IsValid == true)
            {
                bool IsItemValid = true;
                List<EgresosMensualesViewModel> table = SessionState.UserSession.GetDataCollection<List<EgresosMensualesViewModel>>(Constants.FamilyResearch_EgresosMensuales_Table);
                if (table != null)
                {
                    foreach (var iter in table)
                    {
                        if (iter.MovimientoId == model.MovimientoId)
                        {
                            ViewBag.ErrorMessage = "No puede duplicar los egresos";
                            ViewBag.HasErrorMessage = true;
                            IsItemValid = false;
                            break;
                        }
                    }
                }

                if (IsItemValid == true)
                {
                    egresosMensualesViewModel.Key = string.Empty.GetUniqueKey();
                    SessionState.UserSession.AddItemInDataCollection<EgresosMensualesViewModel>(Constants.FamilyResearch_EgresosMensuales_Table, egresosMensualesViewModel);
                }
            }

            return PartialView("_EgresosMensualesForm", egresosMensualesViewModel);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetEgresosMensualesTable")]
        public IActionResult GetEgresosMensualesTable()
        {
            EgresosMensualesViewModel egresosMensualesViewModel = new EgresosMensualesViewModel();
            List<EgresosMensualesViewModel> table = SessionState.UserSession.GetDataCollection<List<EgresosMensualesViewModel>>(Constants.FamilyResearch_EgresosMensuales_Table);
            if (table != null)
            {
                egresosMensualesViewModel.LoadMovimientoList(familyResearchRepository);

                foreach (var iter in table)
                {
                    iter.Movimiento = egresosMensualesViewModel.MovimientoList.Where(m => m.Id == iter.MovimientoId).FirstOrDefault();
                }
            }

            return PartialView("_EgresosMensualesTable", table);
        }

        [HttpPost]
        [Route("/SocialWork/FamilyResearch/RemoveItemInEgresosMensualesTable")]
        public IActionResult RemoveItemInEgresosMensualesTable(string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return BadRequest("key cannot be null or empty");
            }

            var table = SessionState.UserSession.GetDataCollection<List<EgresosMensualesViewModel>>(Constants.FamilyResearch_EgresosMensuales_Table);

            if (table != null)
            {
                var index = table.FindIndex(r => string.Equals(r.Key, key, StringComparison.OrdinalIgnoreCase));

                if (index >= 0 && index < table.Count)
                {
                    SessionState.UserSession.RemoveItemInDataCollection<EgresosMensualesViewModel>(Constants.FamilyResearch_EgresosMensuales_Table, index);
                }
            }

            return Ok();
        }

        private void ClearSessionForTablesObject()
        {
            HttpContext.Session.Remove(Constants.FamilyResearch_IngresosMensuales_Table);
            HttpContext.Session.Remove(Constants.FamilyResearch_EgresosMensuales_Table);
            HttpContext.Session.Remove(Constants.FamilyResearch_FamilyMembers_Table);
            HttpContext.Session.Remove(Constants.FamilyResearch_BenefitsProvided_Table);
        }

        [HttpGet]
        [Route("/SocialWork/FamilyResearch/GetFamilyResearchByMinorName/{minorName}")]
        public IActionResult GetFamilyResearchByMinorName(string minorName)
        {
            if (string.IsNullOrEmpty(minorName))
            {
                return null;
            }

            FamilyResearchViewModel model = new FamilyResearchViewModel();

            if (string.Equals(minorName, "_all_", StringComparison.OrdinalIgnoreCase))
            {
                model.FamilyResearches = familyResearchRepository.GetFamilyResearches()?.ToList();
            }
            else
            {
                model.FamilyResearches = familyResearchRepository.GetFamilyResearchByMinorName(minorName)?.ToList();
            }

            return PartialView("_FamilyResearchTable", model);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        [Route("/SocialWork/FamilyResearch/Print/{id?}")]
        public IActionResult Print(int? Id)
        {
            HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);

            FamilyResearchViewModel model = new FamilyResearchViewModel();
            
            model.SetInitialPatrimonyViewModelCollection(familyResearchRepository);
            model.LoadFoods(familyResearchRepository);
            model.LoadFrequencies(familyResearchRepository);
            model.VisitDate = DateTime.Now;


            if (Id > 0)
            {
                FamilyResearch familyResearch = familyResearchRepository.GetFamilyResearchById((int)Id);
                model.Id = familyResearch.Id;
                model.CaseStudyConclusion = familyResearch.CaseStudyConclusion;
                model.District = familyResearch.District;
                model.DistrictId = familyResearch.DistrictId;
                model.EconomicSituationId = familyResearch.EconomicSituationId;
                model.Family = familyResearch.Family;
                model.FamilyDiagnostic = familyResearch.FamilyDiagnostic;
                model.FamilyExpectations = familyResearch.FamilyExpectations;
                model.FamilyHealth = familyResearch.FamilyHealth;
                model.FamilyHealthId = familyResearch.FamilyHealthId;
                model.FamilyNutrition = familyResearch.FamilyNutrition;
                model.FamilyNutritionId = familyResearch.FamilyNutritionId;
                model.LegalGuardian = familyResearch.LegalGuardian;
                model.LegalGuardianId = familyResearch.LegalGuardianId;
                model.Minor = familyResearch.Minor;
                model.MinorId = familyResearch.MinorId;
                model.Minor.FormalEducation = familyResearch.Minor.FormalEducation;
                model.Minor.FormalEducationId = familyResearch.Minor.FormalEducationId;
                model.PreviousFoundation = familyResearch.PreviousFoundation;
                model.PreviousFoundationId = familyResearch.PreviousFoundationId;
                model.ProblemsIdentified = familyResearch.ProblemsIdentified;
                model.Recommendations = familyResearch.Recommendations;
                model.RedesDeApoyoFamiliares = familyResearch.RedesDeApoyoFamiliares;
                model.RequestReasons = familyResearch.RequestReasons;
                model.SituationsOfDomesticViolence = familyResearch.SituationsOfDomesticViolence;
                model.Sketch = familyResearch.Sketch;
                model.SocioEconomicStudy = familyResearch.SocioEconomicStudy;
                model.SocioEconomicStudyId = familyResearch.SocioEconomicStudyId;
                model.VisualSupports = familyResearch.VisualSupports;
                model.LoadMunicipalitiesOfMexico(familyResearchRepository);
                model.FormVisitTime = familyResearch.VisitTime.ToShortTimeString();
                model.VisitDate = familyResearch.VisitDate;

                model.LoadFamilyNutritionFoodRelation(familyResearch.FamilyNutrition);
                model.LoadPatrimonyViewModelCollection(familyResearch.EconomicSituation);
                model.FamilyMembers = familyResearch.FamilyMembers;
                model.FamilyMembersId = familyResearch.FamilyMembersId;
                model.SetFamilyMembersInSession(familyResearch.FamilyMembers);
                model.BenefitsProvidedList = model.ConvertBenefitsProvidedToBenefitsProvidedViewModel(familyResearch.BenefitsProvided?.BenefitsProvidedDetails);
                model.BenefitsProvidedId = familyResearch.BenefitsProvidedId;
                model.IngresosEgresosMensuales = familyResearch.IngresosEgresosMensuales;
                model.IngresosMensualesList = model.ConvertIngresosEgresosMensualesMovimientoRelationToIngresosMensualesViewModel(familyResearch.IngresosEgresosMensuales?.IngresosEgresosMensualesMovimientoRelation);
                model.IngresosEgresosMensualesId = familyResearch.IngresosEgresosMensualesId;
                model.EgresosMensualesList = model.ConvertIngresosEgresosMensualesMovimientoRelationToEgresosMensualesViewModel(familyResearch.IngresosEgresosMensuales?.IngresosEgresosMensualesMovimientoRelation);

                return View(model);
            }

            return View(model);
        }
    }
}