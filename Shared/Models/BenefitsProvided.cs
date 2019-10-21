using System;

namespace Naandi.Shared.Models
{
    public class BenefitsProvided
    {
        public int Id { get; set; }
        public string Institucion { get; set; }
        public string ApoyoRecibido { get; set; }
        public decimal Monto { get; set; }
        public DateTime Periodo { get; set; }
        public string RedesDeApoyoFamiliares { get; set; }

    }
}