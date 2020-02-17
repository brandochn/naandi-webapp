using System.ComponentModel.DataAnnotations;

namespace Naandi.Shared.Models
{
    public class BenefitsProvidedDetails
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La Institución es requerida")]
        public string Institucion { get; set; }

        [Required(ErrorMessage = "El apoyo recibido es requerido")]
        public string ApoyoRecibido { get; set; }

        [RegularExpression(@"^-?[0-9]*\.?[0-9]+$", ErrorMessage = "Especifque un monto valido")]
        public decimal? Monto { get; set; }
        
        [Required(ErrorMessage = "El periodo es requerido")]
        public string Periodo { get; set; }
        public int BenefitsProvidedId { get; set; }
    }
}