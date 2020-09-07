using System;

namespace Naandi.Shared.Models
{
    public class RegistrationRequest
    {
        public int Id { get; set; }
        public string HowYouHearAboutUs { get; set; }
        public DateTime CreationDate { get; set; }
        public int RequestorId { get; set; }
        public Requestor Requestor { get; set; }
        public int MinorId { get; set; }
        public Minor Minor { get; set; }
        public string Reasons { get; set; }
        public string FamilyComposition { get; set; }
        public string FamilyInteraction { get; set; }
        public string EconomicSituation { get; set; }
        public string SituationsOfDomesticViolence { get; set; }
        public string FamilyHealthStatus { get; set; }
        public string Comments { get; set; }
        public int RegistrationRequestStatusId { get; set; }
        public string SocialWorkerName { get; set; }
        public RegistrationRequestStatus RegistrationRequestStatus { get; set; }
    }
}