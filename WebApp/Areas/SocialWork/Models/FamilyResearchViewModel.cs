using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public PatrimonyViewModelCollection PatrimonyViewModelCollection { get; set; }
        public IList<Food> Foods { get; set; }
        public IList<Frequency> Frequencies { get; set; }
        public int[] FrequencyIdsSelected { get; set; }

        public bool IsValid(object value)
        {
            throw new System.NotImplementedException();
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
            if (string.IsNullOrEmpty(LegalGuardian?.Address?.State))
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
            HomeAcquisitionList = new List<HomeAcquisition>();// registrationRequestRepository.GetStatesOfMexico().ToList();
            HomeAcquisitionList.Insert(0, new HomeAcquisition()
            {
                Name = "Selecciona uno"
            });
        }
        public void LoadTypesOfHousesList(IFamilyResearch familyResearchRepository)
        {
            TypesOfHousesList = new List<TypesOfHouses>();// registrationRequestRepository.GetStatesOfMexico().ToList();
            TypesOfHousesList.Insert(0, new TypesOfHouses()
            {
                Name = "Selecciona uno"
            });
        }
        public void LoadTipoDeMobiliarioList(IFamilyResearch familyResearchRepository)
        {
            TipoDeMobiliarioList = new List<TipoDeMobiliario>();// registrationRequestRepository.GetStatesOfMexico().ToList();
            TipoDeMobiliarioList.Insert(0, new TipoDeMobiliario()
            {
                Name = "Selecciona uno"
            });
        }

        public void LoadTypeOfDistrictList(IFamilyResearch familyResearchRepository)
        {
            TypeOfDistrictList = new List<TypeOfDistrict>();// registrationRequestRepository.GetStatesOfMexico().ToList();
            TypeOfDistrictList.Insert(0, new TypeOfDistrict()
            {
                Name = "Selecciona uno"
            });
        }
        public void LoadPatrimonyViewModelCollection(IFamilyResearch familyResearchRepository)
        {
            PatrimonyViewModelCollection = new PatrimonyViewModelCollection();// registrationRequestRepository.GetStatesOfMexico().ToList();
            PatrimonyViewModelCollection[0] = new PatrimonyViewModel()
            {
                Name = "Automovil",
                Value = string.Empty
            };
        }

        public void LoadFoods(IFamilyResearch familyResearchRepository)
        {
            Foods = new List<Food>();// registrationRequestRepository.GetStatesOfMexico().ToList();
            Foods.Add(new Food()
            {
                Id = 1,
                Name = "Leche"
            });
            Foods.Add(new Food()
            {
                Id = 2,
                Name = "Huevo"
            });
        }

        public void LoadFrequencies(IFamilyResearch familyResearchRepository)
        {
            Frequencies = new List<Frequency>();// registrationRequestRepository.GetStatesOfMexico().ToList();
            Frequencies.Add(new Frequency()
            {
                Id = 1,
                Name = "Diario"
            });
            Frequencies.Add(new Frequency()
            {
                Id = 2,
                Name = "Cada 3er día"
            });

            Frequencies.Insert(0, new Frequency()
            {
                Id = 3,
                Name = "Selecciona uno"
            });

            FrequencyIdsSelected = new int[12];
        }
    }
}