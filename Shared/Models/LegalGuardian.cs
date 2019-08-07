using Naandi.Shared.Models;

namespace Naandi.Shared.Models
{
    public class LegalGuardian
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string PlaceOfBirth { get; set; }
        public MaritalStatus Maritalstatus { get; set; }
        public int MaritalStatusId { get; set; }
        public string Education { get; set; }
        public string CurrentOccupation { get; set; }
        public Relationship Relationship { get; set; }
        public int RelationshipId { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Errand { get; set; }
    }
}