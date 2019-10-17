using System.Collections.Generic;

namespace Naandi.Shared.Models
{
    public class FamilyMembers
    {
        public int Id { get; set; }
        public string FamilyInteraction { get; set; }
        public string Comments { get; set; }
        public FamilyMembersDetails[] FamilyMembersDetails {get; set;}
    }
}