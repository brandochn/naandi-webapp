namespace Naandi.Shared.Models
{
    public class MunicipalitiesOfMexico
    {
        public int Id { get; set; }
        public int StatesOfMexicoId { get; set; }
        public StatesOfMexico StatesOfMexico { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}