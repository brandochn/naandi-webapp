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
        public int[] FrequencyIdsSelected { get; set; }
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
                            Value = "Honda"
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
                    default:
                        throw new Exception("Patrimony not supported");

                }

            }
        }

        public void LoadFoods(IFamilyResearch familyResearchRepository)
        {
            Foods = familyResearchRepository.GetFoods().ToList();
        }

        public void LoadFrequencies(IFamilyResearch familyResearchRepository)
        {
            Frequencies = familyResearchRepository.GetFrequencies().OrderBy(o => o.Id).ToList();
            FrequencyIdsSelected = new int[12];
        }
    }
}