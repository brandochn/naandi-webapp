namespace Naandi.Shared.Models
{
    public class RequestorDocumentRelation
    {
        public int Id { get; set; }
        public int RequestorId { get; set; }
        public int DocumentId { get; set; }
        public byte DocumentReceived { get; set; }
    }
}