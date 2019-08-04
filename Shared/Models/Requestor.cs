using System;

namespace Naandi.Shared.Models
{
    public class Requestor
    {
        public Address Address { get; set; }
        public int AddressId { get; set; }

        public int Age { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Education { get; set; }

        public string FullName { get; set; }
        public int Id { get; set; }
        public Job Job { get; set; }
        public int JobId { get; set; }
        public MaritalStatus Maritalstatus { get; set; }
        public int MaritalStatusId { get; set; }

        public string PlaceOfBirth { get; set; }
        public Relationship Relationship { get; set; }
        public int RelationshipId { get; set; }
        public string CurrentOccupation { get; set; }
    }
}