namespace Naandi.Shared.Models
{
    public class District
    {
        public int Id { get; set; }
        public int TypeOfDistrictId { get; set; }
        public TypeOfDistrict TypeOfDistrict { get; set; }
        public string Aguapotable { get; set; }
        public string Telefono { get; set; }
        public string Electricidad { get; set; }
        public string Drenaje { get; set; }
        public string Hospital { get; set; }
        public string Correo { get; set; }
        public string Escuela { get; set; }
        public string Policia { get; set; }
        public string Alumbradopublico { get; set; }
        public string Viasdeacceso { get; set; }
        public string Transportepublico { get; set; }
        public string Aseopublico { get; set; }
        public string Iglesia { get; set; }
        public string Otros { get; set; }
        public string Description { get; set; }
    }
}