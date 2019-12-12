namespace Naandi.Shared.Models
{
    public class Movimiento
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TipoMovimientoId { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
    }
}