namespace Naandi.Shared.Models
{
    public class EconomicSituation
    {
        public int Id { get; set; }
        public string NivelSocioEconomico { get; set; }
        public EconomicSituationPatrimonyRelation[] EconomicSituationPatrimonyRelation { get; set; }
    }
}