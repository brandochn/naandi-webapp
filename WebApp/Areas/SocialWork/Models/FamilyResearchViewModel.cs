using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApp.ExtensionMethods;
using System.Globalization;

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
        public string FormVisitTime { get; set; }


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

                if (LegalGuardian == null || (DateTime.Now.ToCentralMexicoTime() - LegalGuardian.DateOfBirth).TotalDays > 27375 // 75 years old
                   || (DateTime.Now.ToCentralMexicoTime() - LegalGuardian.DateOfBirth).TotalDays < 1)
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

                if (Minor == null || (DateTime.Now.ToCentralMexicoTime() - Minor.DateOfBirth).TotalDays > 6570 // 18 years old
                    || (DateTime.Now.ToCentralMexicoTime() - Minor.DateOfBirth).TotalDays < 1)
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

                if (DateTime.TryParseExact(FormVisitTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) == false)
                {
                    modelState.AddModelError(string.Empty, $"La hora de visita no es valida: { FormVisitTime ?? "" }");
                    valid = false;
                }

                if (string.IsNullOrEmpty(SocioEconomicStudy?.HouseLayout?.Bedroom))
                {
                    modelState.AddModelError(string.Empty, "Debe agregar una recamara como minimo");
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
        public void SetInitialPatrimonyViewModelCollection(IFamilyResearch familyResearchRepository)
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

        public void GetEconomicSituationPatrimonyRelationFromViewModel(IFamilyResearch familyResearchRepository)
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

            if (EconomicSituationId > 0)
            {
                EconomicSituation.Id = Convert.ToInt32(EconomicSituationId);
                EconomicSituation.EconomicSituationPatrimonyRelation[0].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[1].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[2].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[3].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[4].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[5].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[6].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[7].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[8].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[9].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[10].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[11].EconomicSituationId = EconomicSituation.Id;
                EconomicSituation.EconomicSituationPatrimonyRelation[12].EconomicSituationId = EconomicSituation.Id;
            }

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

        public void GetFamilyNutritionFoodRelationFromViewModel(IFamilyResearch familyResearchRepository)
        {
            var _foods = familyResearchRepository.GetFoods().OrderBy(o => o.Name).ToList();

            if (FamilyNutrition == null)
            {
                FamilyNutrition = new FamilyNutrition();
            }

            if (FamilyNutritionId > 0)
            {
                FamilyNutrition.Id = Convert.ToInt32(FamilyNutritionId);
            }

            FamilyNutrition.FamilyNutritionFoodRelation = new FamilyNutritionFoodRelation[_foods.Count];
            for (int index = 0; index < FrequencyIdsSelected.Length; index++)
            {
                FamilyNutrition.FamilyNutritionFoodRelation[index] = new FamilyNutritionFoodRelation();
                FamilyNutrition.FamilyNutritionFoodRelation[index].FoodId = _foods[index].Id;
                FamilyNutrition.FamilyNutritionFoodRelation[index].FrequencyId = FrequencyIdsSelected[index].Id;
                if (FamilyNutritionId > 0)
                {
                    FamilyNutrition.FamilyNutritionFoodRelation[index].FamilyNutritionId = FamilyNutrition.Id;
                }
            }
        }

        public void GetBenefitsProvidedFromSession()
        {
            List<BenefitsProvidedViewModel> collection = SessionState.UserSession.GetDataCollection<List<BenefitsProvidedViewModel>>(Constants.FamilyResearch_BenefitsProvided_Table);
            if (collection != null)
            {
                BenefitsProvided = new BenefitsProvided();
                BenefitsProvided.BenefitsProvidedDetails = new BenefitsProvidedDetails[collection.Count];

                if (BenefitsProvidedId > 0)
                {
                    BenefitsProvided.Id = Convert.ToInt32(BenefitsProvidedId);
                }

                for (int index = 0; index < collection.Count; index++)
                {
                    BenefitsProvided.BenefitsProvidedDetails[index] = collection[index];
                    if (BenefitsProvidedId > 0)
                    {
                        BenefitsProvided.BenefitsProvidedDetails[index].BenefitsProvidedId = BenefitsProvided.Id;
                    }
                }
            }
        }

        public void GetIngresosMensualesFromSession()
        {
            List<IngresosMensualesViewModel> ingresosCollection = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_IngresosMensuales_Table);
            List<EgresosMensualesViewModel> egresosCollection = SessionState.UserSession.GetDataCollection<List<EgresosMensualesViewModel>>(Constants.FamilyResearch_EgresosMensuales_Table);

            int size = ingresosCollection == null ? 0 : ingresosCollection.Count;
            size = egresosCollection == null ? size : (size + egresosCollection.Count);

            if (IngresosEgresosMensuales == null)
            {
                IngresosEgresosMensuales = new IngresosEgresosMensuales();
            }
            IngresosEgresosMensuales.IngresosEgresosMensualesMovimientoRelation = new IngresosEgresosMensualesMovimientoRelation[size];
            int uniqueIndex = 0;

            if (ingresosCollection == null && egresosCollection == null)
            {
                IngresosEgresosMensuales = null;
                return;
            }

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

        public void GetFamilyMembersFromSession()
        {
            var collection = SessionState.UserSession.GetDataCollection<List<FamilyMembersDetails>>(Constants.FamilyResearch_FamilyMembers_Table);
            if (collection != null)
            {
                FamilyMembers = new FamilyMembers();
                FamilyMembers.FamilyMembersDetails = new FamilyMembersDetails[collection.Count];
                if (FamilyMembersId > 0)
                {
                    FamilyMembers.Id = Convert.ToInt32(FamilyMembersId);
                }

                for (int index = 0; index < collection.Count; index++)
                {
                    FamilyMembers.FamilyMembersDetails[index] = collection[index];
                    if (FamilyMembersId > 0)
                    {
                        FamilyMembers.FamilyMembersDetails[index].FamilyMembersId = FamilyMembers.Id;
                    }
                }
            }
        }

        public List<IngresosMensualesViewModel> ConvertIngresosEgresosMensualesMovimientoRelationToIngresosMensualesViewModel(IngresosEgresosMensualesMovimientoRelation[] from)
        {
            if (from == null)
            {
                return null;
            }

            List<IngresosMensualesViewModel> to = new List<IngresosMensualesViewModel>();
            foreach (var iter in from)
            {
                if (iter.Movimiento.TipoMovimiento.Name == "Ingreso")
                {
                    IngresosMensualesViewModel ingresosMensuales = new IngresosMensualesViewModel()
                    {
                        Id = iter.Id,
                        Key = string.Empty.GetUniqueKey(),
                        IngresosEgresosMensualesId = iter.IngresosEgresosMensualesId,
                        Monto = iter.Monto,
                        Movimiento = iter.Movimiento,
                        MovimientoId = iter.MovimientoId,
                    };

                    to.Add(ingresosMensuales);
                }
            }

            List<IngresosMensualesViewModel> collection = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_IngresosMensuales_Table);
            if (collection == null)
            {
                foreach (var iter in to)
                {
                    SessionState.UserSession.AddItemInDataCollection(Constants.FamilyResearch_IngresosMensuales_Table, iter);

                }
            }

            return to;
        }

        public List<EgresosMensualesViewModel> ConvertIngresosEgresosMensualesMovimientoRelationToEgresosMensualesViewModel(IngresosEgresosMensualesMovimientoRelation[] from)
        {
            if (from == null)
            {
                return null;
            }

            List<EgresosMensualesViewModel> to = new List<EgresosMensualesViewModel>();
            foreach (var iter in from)
            {
                if (iter.Movimiento.TipoMovimiento.Name == "Egreso")
                {
                    EgresosMensualesViewModel egresosMensuales = new EgresosMensualesViewModel()
                    {
                        Id = iter.Id,
                        Key = string.Empty.GetUniqueKey(),
                        IngresosEgresosMensualesId = iter.IngresosEgresosMensualesId,
                        Monto = iter.Monto,
                        Movimiento = iter.Movimiento,
                        MovimientoId = iter.MovimientoId
                    };

                    to.Add(egresosMensuales);
                }
            }

            List<IngresosMensualesViewModel> collection = SessionState.UserSession.GetDataCollection<List<IngresosMensualesViewModel>>(Constants.FamilyResearch_EgresosMensuales_Table);
            if (collection == null)
            {
                foreach (var iter in to)
                {
                    SessionState.UserSession.AddItemInDataCollection(Constants.FamilyResearch_EgresosMensuales_Table, iter);
                }
            }

            return to;
        }

        public void SetFamilyMembersInSession(FamilyMembers familyMembers)
        {
            if (familyMembers?.FamilyMembersDetails == null)
            {
                return;
            }

            var collection = SessionState.UserSession.GetDataCollection<List<FamilyMembersDetails>>(Constants.FamilyResearch_FamilyMembers_Table);
            if (collection == null)
            {
                foreach (var iter in familyMembers.FamilyMembersDetails)
                {
                    SessionState.UserSession.AddItemInDataCollection<FamilyMembersDetails>(Constants.FamilyResearch_FamilyMembers_Table, iter);
                }
            }
        }

        public List<BenefitsProvidedViewModel> ConvertBenefitsProvidedToBenefitsProvidedViewModel(BenefitsProvidedDetails[] from)
        {
            if (from == null)
            {
                return null;
            }

            List<BenefitsProvidedViewModel> to = new List<BenefitsProvidedViewModel>();
            foreach (var iter in from)
            {
                BenefitsProvidedViewModel benefitsProvided = new BenefitsProvidedViewModel()
                {
                    Id = iter.Id,
                    Key = string.Empty.GetUniqueKey(),
                    ApoyoRecibido = iter.ApoyoRecibido,
                    BenefitsProvidedId = iter.BenefitsProvidedId,
                    Institucion = iter.Institucion,
                    Monto = iter.Monto,
                    Periodo = iter.Periodo
                };

                to.Add(benefitsProvided);
            }

            List<BenefitsProvidedViewModel> collection = SessionState.UserSession.GetDataCollection<List<BenefitsProvidedViewModel>>(Constants.FamilyResearch_BenefitsProvided_Table);
            if (collection == null)
            {
                foreach (var iter in to)
                {
                    SessionState.UserSession.AddItemInDataCollection<BenefitsProvidedViewModel>(Constants.FamilyResearch_BenefitsProvided_Table, iter);
                }
            }

            return to;
        }

        public void LoadPatrimonyViewModelCollection(EconomicSituation economicSituation)
        {
            if (economicSituation?.EconomicSituationPatrimonyRelation == null || economicSituation.EconomicSituationPatrimonyRelation.Length == 0)
            {
                return;
            }

            PatrimonyViewModelCollection = new List<PatrimonyViewModel>();
            foreach (var iter in economicSituation.EconomicSituationPatrimonyRelation)
            {
                PatrimonyViewModelCollection.Add(new PatrimonyViewModel()
                {
                    Id = iter.Id,
                    Name = iter.Patrimony.Name,
                    Value = iter.Value
                });
            }
        }

        public void LoadFamilyNutritionFoodRelation(FamilyNutrition familyNutrition)
        {
            if (familyNutrition?.FamilyNutritionFoodRelation == null || familyNutrition.FamilyNutritionFoodRelation.Length == 0)
            {
                return;
            }

            var frequencies = new List<Frequency>();
            foreach (var iter in familyNutrition.FamilyNutritionFoodRelation)
            {
                frequencies.Add(new Frequency()
                {
                    Id = iter.FrequencyId,
                    Name = Frequencies.Where(f => f.Id == iter.FrequencyId).FirstOrDefault()?.Name
                }) ;
            }

            FrequencyIdsSelected = frequencies.ToArray();
        }
    }
}