namespace Naandi.Shared.Models
{
    public class FormalEducation
    {
        public int Id { get; set; }
        public bool CanItRead { get; set; }
        public bool CanItWrite { get; set; }
        public bool IsItStudyingNow { get; set; }
        public string CurrentGrade { get; set; }
        public string ReasonsToStopStudying { get; set; }
    }
}