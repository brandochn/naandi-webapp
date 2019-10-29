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
    }
}