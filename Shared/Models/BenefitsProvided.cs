using System;

namespace Naandi.Shared.Models
{
    public class BenefitsProvided
    {
        public int Id { get; set; }
        public string RedesDeApoyoFamiliares { get; set; }
        public BenefitsProvidedDetails[] BenefitsProvidedDetails { get; set; }
    }
}