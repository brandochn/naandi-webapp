namespace Naandi.Shared.Models
{
    public class Economicsituation
    {
        public int Id { get; set; }
        public string Nivelsocioeconomico { get; set; }
        EconomicSituationPatrimonyRelation[] EconomicSituationPatrimonyRelation { get; set; }
    }
}