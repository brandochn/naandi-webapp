namespace Naandi.Shared.Models
{
    public class SocioEconomicStudy
    {
        public int Id { get; set; }
        public string Vivienda { get; set; }
        public string NombrePropietario { get; set; }
        public string MedioAdquisicion { get; set; }
        public int TypesOfHousesId { get; set; }
        public TypesOfHouses TypesOfHouses {get; set;}
        public int HouseLayoutId { get; set; }
        public HouseLayout HouseLayout {get; set;}
    }
}