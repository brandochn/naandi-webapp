using Naandi.Shared.Models;
using Naandi.Shared.Services;
using System;
using System.Collections.Generic;

namespace WebApp.Areas.SocialWork.Models
{
    public class FamilyResearchViewModel : FamilyResearch, IBaseViewModel
    {
        public IList<MaritalStatus> MaritalStatusList { get; set; }
        public IList<Relationship> RelationshipList { get; set; }
        public IList<StatesOfMexico> StatesOfMexico { get; set; }
        public IList<MunicipalitiesOfMexico> MunicipalitiesOfMexico { get; set; }
        public FamilyMembersDetails FamilyMember { get; set; }
        public IList<HomeAcquisition> HomeAcquisitionList { get; set; }


        public bool IsValid(object value)
        {
            throw new System.NotImplementedException();
        }

        public void LoadMaritalStatuses(IFamilyResearch familyResearchRepository)
        {
            // MaritalStatusList = familyResearchRepository.GetMaritalStatuses().ToList();
            MaritalStatusList = new List<MaritalStatus>();
            MaritalStatusList.Insert(0, new MaritalStatus()
            {
                Id = 0,
                Name = "Selecciona estado civil"
            });
        }

        public void LoadRelationships(IFamilyResearch familyResearchRepository)
        {
            //RelationshipList = familyResearchRepository.GetRelationships().ToList();
            RelationshipList = new List<Relationship>();
            RelationshipList.Insert(0, new Relationship()
            {
                Id = 0,
                Name = "Selecciona parentesco"
            });
        }

        public void LoadStatesOfMexico(IFamilyResearch familyResearchRepository)
        {
            StatesOfMexico = new List<StatesOfMexico>();// registrationRequestRepository.GetStatesOfMexico().ToList();
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
                MunicipalitiesOfMexico = new List<MunicipalitiesOfMexico>(); /* registrationRequestRepository
                    .GetMunicipalitiesOfMexicoByStateOfMexicoName(Requestor.Address.State).ToList(); */
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
    }
}