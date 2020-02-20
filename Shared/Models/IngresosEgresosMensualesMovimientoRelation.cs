using System.ComponentModel.DataAnnotations;

namespace Naandi.Shared.Models
{
    public class IngresosEgresosMensualesMovimientoRelation
    {
        public int Id { get; set; }
        public int IngresosEgresosMensualesId { get; set; }
        
        [Range(1, 20, ErrorMessage = "El tipo de movimiento es requerido")]
        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        public int? MovimientoId { get; set; }
        public Movimiento Movimiento { get; set; }

        [RegularExpression(@"^-?[0-9]*\.?[0-9]+$", ErrorMessage = "Especifque un monto valido")]
        [Required(ErrorMessage = "El monto es requerido")]
        public decimal? Monto { get; set; }
    }
}