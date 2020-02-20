using System.ComponentModel.DataAnnotations;

namespace Naandi.Shared.Models
{
    public class FamilyMembersDetails
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido")]
        public string FullName { get; set; }

        [Range(0, 100, ErrorMessage = "La edad esta fuera de rango")]
        public int? Age { get; set; }

        
        [Range(1, 20, ErrorMessage = "El estado civil es requerido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El estado civil es requerido")]
        public int? MaritalStatusId { get; set; }

        public MaritalStatus MaritalStatus { get; set; }

        [Range(1, 20, ErrorMessage = "El parentesco es requerido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El parentesco es requerido")]
        public int? RelationshipId { get; set; }
        
        public Relationship Relationship { get; set; }
        public string Education { get; set; }
        public string CurrentOccupation { get; set; }
        public int FamilyMembersId { get; set; }
    }

}