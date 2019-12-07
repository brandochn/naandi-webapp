namespace Naandi.Shared.Models
{
    public class BenefitsProvidedDetails
    {
        public int Id { get; set; }
        public string Institucion { get; set; }
        public string ApoyoRecibido { get; set; }
        public decimal Monto { get; set; }
        public string Periodo { get; set; }
        public int BenefitsProvidedId { get; set; }
    }
}