namespace Naandi.Shared.Models
{
    public class MinorDocumentRelation
    {
        public int Id { get; set; }
        public int MinorId { get; set; }
        public int DocumentId { get; set; }
        public bool DocumentReceived { get; set; }
    }
}