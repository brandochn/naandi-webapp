namespace Naandi.Shared.Models
{
    public class IngresosEgresosMensualesMovimientoRelation
    {
        public int Id { get; set; }
        public int IngresosEgresosMensualesId { get; set; }
        public int MovimientoId { get; set; }
        public decimal Monto { get; set; }
    }
}