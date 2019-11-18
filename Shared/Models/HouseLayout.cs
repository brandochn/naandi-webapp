namespace Naandi.Shared.Models
{
    public class HouseLayout
    {
        public int Id { get; set; }
        public string Bedroom { get; set; }
        public string Dinningroom { get; set; }
        public string Kitchen { get; set; }
        public string Livingroom { get; set; }
        public string Bathroom { get; set; }
        public string Patio { get; set; }
        public string Garage { get; set; }
        public string Backyard { get; set; }
        public string Other { get; set; }
        public string Ground { get; set; }
        public string Walls { get; set; }
        public string Roof { get; set; }
        public string Description { get; set; }
        public int TipoDeMobiliarioId { get; set; }
        public TipoDeMobiliario TipoDeMobiliario { get; set; }
        public string CharacteristicsOfFurniture { get; set; }
    }
}