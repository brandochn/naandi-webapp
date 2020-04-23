namespace Naandi.Shared.Models
{
    public class EconomicSituationPatrimonyRelation
    {
        public int Id { get; set; }
        public int EconomicSituationId { get; set; }
        public int PatrimonyId { get; set; }

        public Patrimony Patrimony { get; set; }
        public string Value { get; set; }
    }
}
