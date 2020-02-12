using System.Collections.Generic;
using System.Linq;
using Naandi.Shared.Models;
using Naandi.Shared.Services;

namespace WebApp.Areas.SocialWork.Models
{
    public class FamilyMembersViewModel : FamilyMembersDetails
    {
        public IList<MaritalStatus> MaritalStatusList { get; set; }
        public IList<Relationship> RelationshipList { get; set; }

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
    }
}