using System;

namespace Naandi.Shared.Models
{
    public class EntryRegister
    {
        public int Id { get; set; }
        public int RequestorId { get; set; }
        public Requestor Requestor { get; set; }
        public int MinorId { get; set; }
        public Minor Minor { get; set; }
        public DateTime Creationdate { get; set; }
        public string SignedBy { get; set; }
    }
}