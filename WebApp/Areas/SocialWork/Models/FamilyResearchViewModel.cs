using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Areas.SocialWork.Models
{
    public class FamilyResearchViewModel : FamilyResearch, IBaseViewModel
    {
        public IList<MaritalStatus> MaritalStatusList { get; set; }
        public IList<Relationship> RelationshipList { get; set; }
        public IList<StatesOfMexico> StatesOfMexico { get; set; }
        public IList<MunicipalitiesOfMexico> MunicipalitiesOfMexico { get; set; }
        public IList<HomeAcquisition> HomeAcquisitionList { get; set; }
        public IList<TypesOfHouses> TypesOfHousesList { get; set; }
        public IList<TipoDeMobiliario> TipoDeMobiliarioList { get; set; }
        public IList<TypeOfDistrict> TypeOfDistrictList { get; set; }
        public List<PatrimonyViewModel> PatrimonyViewModelCollection { get; set; }
        public IList<Food> Foods { get; set; }
        public IList<Frequency> Frequencies { get; set; }
        public Frequency[] FrequencyIdsSelected { get; set; }
        public IList<FamilyResearch> FamilyResearches { get; set; }
        public IList<BenefitsProvidedViewModel> BenefitsProvidedList { get; set; }
        public IList<IngresosMensualesViewModel> IngresosMensualesList { get; set; }
        public IList<EgresosMensualesViewModel> EgresosMensualesList { get; set; }


        public bool IsValid(object value)
        {
            var valid = true;
            var modelState = value as ModelStateDictionary;
            if (modelState != null)
            {
                if (string.IsNullOrEmpty(LegalGuardian?.FullName))
                {
                    modelState.AddModelError(string.Empty, "El nombre del tutor es requerido");
                    valid = false;
                }

                if (LegalGuardian == null || LegalGuardian.Age <= 0 || LegalGuardian.Age > 100)
                {
                    modelState.AddModelError(string.Empty, "La edad del tutor deber ser un número entre 1 al 100");
                    valid = false;
                }

                if (LegalGuardian == null || LegalGuardian.MaritalStatusId == 0)
                {
                    modelState.AddModelError(string.Empty, "El estado civil es requerido");
                    valid = false;
                }

                if (LegalGuardian == null || LegalGuardian.RelationshipId == 0)
                {
                    modelState.AddModelError(string.Empty, "El parentesco es requerido");
                    valid = false;
                }

                if (string.IsNullOrEmpty(LegalGuardian?.PlaceOfBirth))
                {
                    modelState.AddModelError(string.Empty, "El lugar de nacimiento del solicitante es requerido");
                    valid = false;
                }

                if (LegalGuardian == null || (DateTime.Now - LegalGuardian.DateOfBirth).TotalDays > 27375 // 75 years old
                   || (DateTime.Now - LegalGuardian.DateOfBirth).TotalDays < 1)
                {
                    modelState.AddModelError(string.Empty, "La fecha de nacimiento del tutor no es valida");
                    valid = false;
                }

                if (string.IsNullOrEmpty(LegalGuardian?.Address?.Street))
                {
                    modelState.AddModelError(string.Empty, "La dirección del tutor es requerido");
                    valid = false;
                }

                if (string.IsNullOrEmpty(LegalGuardian?.Address?.City))
                {
                    modelState.AddModelError(string.Empty, "El municipio del tutor es requerido");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Minor?.FullName))
                {
                    modelState.AddModelError(string.Empty, "El nombre de la menor es requerido");
                    valid = false;
                }

                if (Minor == null || (DateTime.Now - Minor.DateOfBirth).TotalDays > 6570 // 18 years old
                    || (DateTime.Now - Minor.DateOfBirth).TotalDays < 1)
                {
                    modelState.AddModelError(string.Empty, "La fecha de nacimiento de la menor no es valida");
                    valid = false;
                }

                if (Minor == null || Minor.Age <= 0 || Minor.Age > 100)
                {
                    modelState.AddModelError(string.Empty, "La edad de la menor deber ser un número entre 1 al 100");
                    valid = false;
                }

                if (string.IsNullOrEmpty(Minor?.PlaceOfBirth))
                {
                    modelState.AddModelError(string.Empty, "El lugar de nacimiento de la menor es requerido");
                    valid = false;
                }

                if (SocioEconomicStudy == null || SocioEconomicStudy.TypesOfHousesId == 0)
                {
                    modelState.AddModelError(string.Empty, "El tipo de vivienda es requerido");
                    valid = false;
                }

                if (SocioEconomicStudy == null || SocioEconomicStudy.HomeAcquisitionId == 0)
                {
                    modelState.AddModelError(string.Empty, "Vivienda es requerido");
                    valid = false;
                }

                if (SocioEconomicStudy == null ||
                    SocioEconomicStudy.HouseLayout == null ||
                    SocioEconomicStudy.HouseLayout.TipoDeMobiliarioId == 0)
                {
                    modelState.AddModelError(string.Empty, "Tipo de mobiliario es requerido");
                    valid = false;
                }

                if (District == null ||
                   District.TypeOfDistrictId == 0)
                {
                    modelState.AddModelError(string.Empty, "Tipo de colonia es requerido");
                    valid = false;
                }
            }

            return valid;
        }

        public void LoadMaritalStatuses(IFamilyResearch familyResearchRepository)
        {
            MaritalStatusList = familyResearchRepository.GetMaritalStatuses().ToList();
            MaritalStatusList.Insert(0, new MaritalStatus()
            {
                Id = 0,
                Name = "Selecciona estado civil"
            });
        }

        public void LoadRelationships(IFamilyResearch familyResearchRepository)
        {
            RelationshipList = familyResearchRepository.GetRelationships().ToList();
            RelationshipList.Insert(0, new Relationship()
            {
                Id = 0,
                Name = "Selecciona parentesco"
            });
        }

        public void LoadStatesOfMexico(IFamilyResearch familyResearchRepository)
        {
            StatesOfMexico = familyResearchRepository.GetStatesOfMexico().ToList();
            StatesOfMexico.Insert(0, new StatesOfMexico()
            {
                Nombre = "Selecciona un estado"
            });
        }

        public void LoadMunicipalitiesOfMexico(IFamilyResearch familyResearchRepository)
        {
            if (string.IsNullOrEmpty(LegalGuardian?.Address?.State) ||
                LegalGuardian?.Address?.State.StartsWith("Selecciona") == true)
            {
                MunicipalitiesOfMexico = new List<MunicipalitiesOfMexico>();
                MunicipalitiesOfMexico.Add(new MunicipalitiesOfMexico()
                {
                    Nombre = "Selecciona un municipio"
                });
            }
            else
            {
                MunicipalitiesOfMexico = familyResearchRepository
                    .GetMunicipalitiesOfMexicoByStateOfMexicoName(LegalGuardian.Address.State).ToList();
            }
        }

        public void LoadHomeAcquisitionList(IFamilyResearch familyResearchRepository)
        {
            HomeAcquisitionList = familyResearchRepository.GetHomeAcquisitions().ToList();
            HomeAcquisitionList.Insert(0, new HomeAcquisition()
            {
                Name = "Selecciona uno"
            });
        }
        public void LoadTypesOfHousesList(IFamilyResearch familyResearchRepository)
        {
            TypesOfHousesList = familyResearchRepository.GetTypesOfHouses().ToList();
            TypesOfHousesList.Insert(0, new TypesOfHouses()
            {
                Name = "Selecciona uno"
            });
        }
        public void LoadTipoDeMobiliarioList(IFamilyResearch familyResearchRepository)
        {
            TipoDeMobiliarioList = familyResearchRepository.GetTipoDeMobiliarios().ToList();
            TipoDeMobiliarioList.Insert(0, new TipoDeMobiliario()
            {
                Name = "Selecciona uno"
            });
        }

        public void LoadTypeOfDistrictList(IFamilyResearch familyResearchRepository)
        {
            TypeOfDistrictList = familyResearchRepository.GetTypeOfDistricts().ToList();
            TypeOfDistrictList.Insert(0, new TypeOfDistrict()
            {
                Name = "Selecciona uno"
            });
        }
        public void LoadPatrimonyViewModelCollection(IFamilyResearch familyResearchRepository)
        {
            var patrimonies = familyResearchRepository.GetPatrimonies()?.OrderBy(o => o.Id)?.ToList();
            if (patrimonies == null)
            {
                return;
            }

            PatrimonyViewModelCollection = new List<PatrimonyViewModel>();
            foreach (var p in patrimonies)
            {
                switch (p.Name)
                {
                    case "Automovil":
                        PatrimonyViewModelCollection.Insert(0, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "Modelo":

                        PatrimonyViewModelCollection.Insert(1, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "CasaHabitacion":

                        PatrimonyViewModelCollection.Insert(2, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "CasaHabitacionUbicacion":

                        PatrimonyViewModelCollection.Insert(3, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "Terreno":

                        PatrimonyViewModelCollection.Insert(4, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "TerrenoUbicacion":

                        PatrimonyViewModelCollection.Insert(5, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "Otros":

                        PatrimonyViewModelCollection.Insert(6, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "Ahorros":

                        PatrimonyViewModelCollection.Insert(7, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "FrecuenciaDeAhorro":

                        PatrimonyViewModelCollection.Insert(8, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "AutomovilValor":
                        PatrimonyViewModelCollection.Insert(9, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "CasaHabitacionValor":
                        PatrimonyViewModelCollection.Insert(10, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "TerrenoValor":
                        PatrimonyViewModelCollection.Insert(11, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    case "AhorrosValor":
                        PatrimonyViewModelCollection.Insert(12, new PatrimonyViewModel()
                        {
                            Name = p.Name,
                            Value = string.Empty
                        });
                        break;
                    default:
                        throw new Exception("Patrimony not supported");

                }

            }
        }

        public void LoadFoods(IFamilyResearch familyResearchRepository)
        {
            Foods = familyResearchRepository.GetFoods().OrderBy(o => o.Name).ToList();
        }

        public void LoadFrequencies(IFamilyResearch familyResearchRepository)
        {
            Frequencies = familyResearchRepository.GetFrequencies().OrderBy(o => o.Id).ToList();
            int size = Foods.Count;
            FrequencyIdsSelected = new Frequency[size];
        }

        public void LoadEconomicSituationPatrimonyRelationFromSession(IFamilyResearch familyResearchRepository)
        {
            var patrimonies = familyResearchRepository.GetPatrimonies()?.ToList();
            if (patrimonies == null)
            {
                return;
            }

            if (EconomicSituation == null)
            {
                EconomicSituation = new EconomicSituation();
            }

            EconomicSituation.EconomicSituationPatrimonyRelation = new EconomicSituationPatrimonyRelation[PatrimonyViewModelCollection.Count];
            EconomicSituation.EconomicSituationPatrimonyRelation[0] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[1] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[2] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[3] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[4] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[5] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[6] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[7] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[8] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[9] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[10] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[11] = new EconomicSituationPatrimonyRelation();
            EconomicSituation.EconomicSituationPatrimonyRelation[12] = new EconomicSituationPatrimonyRelation();


            EconomicSituation.EconomicSituationPatrimonyRelation[0].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "Automovil", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[0].Value = PatrimonyViewModelCollection[0].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[1].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "Modelo", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[1].Value = PatrimonyViewModelCollection[1].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[9].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "AutomovilValor", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[9].Value = PatrimonyViewModelCollection[9].Value;

            EconomicSituation.EconomicSituationPatrimonyRelation[2].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "CasaHabitacion", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[2].Value = PatrimonyViewModelCollection[2].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[3].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "CasaHabitacionUbicacion", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[3].Value = PatrimonyViewModelCollection[3].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[10].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "CasaHabitacionValor", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[10].Value = PatrimonyViewModelCollection[10].Value;

            EconomicSituation.EconomicSituationPatrimonyRelation[4].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "Terreno", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[4].Value = PatrimonyViewModelCollection[4].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[5].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "TerrenoUbicacion", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[5].Value = PatrimonyViewModelCollection[5].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[11].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "TerrenoValor", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[11].Value = PatrimonyViewModelCollection[11].Value;

            EconomicSituation.EconomicSituationPatrimonyRelation[6].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "Otros", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[6].Value = PatrimonyViewModelCollection[6].Value;

            EconomicSituation.EconomicSituationPatrimonyRelation[7].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "Ahorros", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[7].Value = PatrimonyViewModelCollection[7].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[8].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "FrecuenciaDeAhorro", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[8].Value = PatrimonyViewModelCollection[8].Value;
            EconomicSituation.EconomicSituationPatrimonyRelation[12].PatrimonyId = patrimonies.First(p => string.Equals(p.Name, "AhorrosValor", StringComparison.OrdinalIgnoreCase)).Id;
            EconomicSituation.EconomicSituationPatrimonyRelation[12].Value = PatrimonyViewModelCollection[12].Value;
        }

        public void LoadFamilyNutritionFoodRelationFromSession(IFamilyResearch familyResearchRepository)
        {
            var _foods = familyResearchRepository.GetFoods().OrderBy(o => o.Name).ToList();

            if (FamilyNutrition == null)
            {
                FamilyNutrition = new FamilyNutrition();
            }

            FamilyNutrition.FamilyNutritionFoodRelation = new FamilyNutritionFoodRelation[_foods.Count];
            for (int index = 0; index < FrequencyIdsSelected.Length; index++)
            {
                FamilyNutrition.FamilyNutritionFoodRelation[index] = new FamilyNutritionFoodRelation();
                FamilyNutrition.FamilyNutritionFoodRelation[index].FoodId = _foods[index].Id;
                FamilyNutrition.FamilyNutritionFoodRelation[index].FrequencyId = FrequencyIdsSelected[index].Id;
            }
        }

        public void LoadBenefitsProvidedFromSession()
        {
            List<BenefitsProvidedViewModel> collection = SessionState.UserSession.GetDataCollection<List<BenefitsProvidedViewModel>>(Constants.FamilyResearch_BenefitsProvided_Table);
            if (collection != null)
            {
                BenefitsProvided = new BenefitsProvided();
                BenefitsProvided.BenefitsProvidedDetails = new BenefitsProvidedDetails[collection.Count];
                for (int index = 0; index < collection.Count; index++)
                {
                    BenefitsProvided.BenefitsProvidedDetails[index] = collection[index];
                }
            }
        }

        public void LoadIngresosMensualesFromSession()
        {
            List<IngresosMensualesViewModel> ingresosCollection = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_IngresosMensuales_Table);
            List<EgresosMensualesViewModel> egresosCollection = SessionState.UserSession.GetDataCollection<List<EgresosMensualesViewModel>>(Constants.FamilyResearch_EgresosMensuales_Table);

            int size = ingresosCollection == null ? 0 : ingresosCollection.Count;
            size = egresosCollection == null ? size : (size + egresosCollection.Count);
            IngresosEgresosMensuales = new IngresosEgresosMensuales();
            IngresosEgresosMensuales.IngresosEgresosMensualesMovimientoRelation = new IngresosEgresosMensualesMovimientoRelation[size];
            int uniqueIndex = 0;

            if (ingresosCollection != null)
            {
                for (int index = 0; index < ingresosCollection.Count; index++)
                {
                    IngresosEgresosMensuales.IngresosEgresosMensualesMovimientoRelation[index] = ingresosCollection[index];
                    uniqueIndex = index;
                }
            }

            if (egresosCollection != null)
            {
                if (uniqueIndex == 0)
                {
                    for (int index = 0; index < egresosCollection.Count; index++)
                    {
                        IngresosEgresosMensuales.IngresosEgresosMensualesMovimientoRelation[index] = egresosCollection[index];
                    }
                }
                else
                {
                    for (int index = 0; index < egresosCollection.Count; index++)
                    {
                        uniqueIndex += 1;
                        IngresosEgresosMensuales.IngresosEgresosMensualesMovimientoRelation[uniqueIndex] = egresosCollection[index];
                    }
                }
            }
        }

        public void LoadFamilyMembers()
        {
            var collection =  SessionState.UserSession.GetDataCollection<List<FamilyMembersDetails>>(Constants.FamilyResearch_FamilyMembers_Table);
            if (collection != null)
            {
                FamilyMembers = new FamilyMembers();
                FamilyMembers.FamilyMembersDetails = new FamilyMembersDetails[collection.Count];
                for (int index = 0; index < collection.Count; index++)
                {
                    FamilyMembers.FamilyMembersDetails[index] = collection[index];
                }
            }
        }
    }
}