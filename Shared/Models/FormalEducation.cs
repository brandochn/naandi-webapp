namespace Naandi.Shared.Models
{
    public class FormalEducation
    {
        public int Id { get; set; }
        public byte CanItRead { get; set; }
        public byte CanItWrite { get; set; }
        public byte IsItStudyingNow { get; set; }
        public string CurrentGrade { get; set; }
        public string ReasonsToStopStudying { get; set; }
    }
}