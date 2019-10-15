namespace Naandi.Shared.Models
{
    public class FamilyMembersDetails
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int MaritalStatusId { get; set; }
        public int RelationshipId { get; set; }
        public string Education { get; set; }
        public string CurrentOccupation { get; set; }
        public int FamilyMembersId { get; set; }
    }

}