namespace Naandi.Shared.Models
{
    public class IngresosEgresosMensuales
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public IngresosEgresosMensualesMovimientoRelation[] IngresosEgresosMensualesMovimientoRelation { get; set; }
    }
}