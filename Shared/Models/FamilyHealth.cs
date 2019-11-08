namespace Naandi.Shared.Models
{
    public class FamilyHealth
    {
        public int Id { get; set; }
        public string FamilyHealthStatus { get; set; }
        public string DerechoHambienteAServiciosDeSalud { get; set; }
        public string Tipo { get; set; }
        public string EnfermedadesCronicasDegenerativas { get; set; }
        public string ConsumoDeTabaco { get; set; }
        public string ConsumoDeAlcohol { get; set; }
        public string ConsumoDeDrogas { get; set; }
        public string Comments { get; set; }
    }
}