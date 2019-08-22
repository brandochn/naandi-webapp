namespace Naandi.Shared.Models
{
    public class MunicipalitiesOfMexico
    {
        public int Id { get; set; }
        public int EstadoId { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public byte Activo { get; set; }

        public StatesOfMexico StatesOfMexico { get; set; }
    }
}