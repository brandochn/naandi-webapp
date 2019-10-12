using System;

namespace Naandi.Shared.Models
{
    public class Minor
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public int Age { get; set; }
        public string Education { get; set; }
        public string CurrentOccupation { get; set; }
        public int FormalEducationId { get; set; }
        public FormalEducation FormalEducation  {get; set;}
    }
}