using System;

namespace Naandi.Shared.Models
{
    public class FamilyResearch
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime CreationTime { get; set; }
        public string Family { get; set; }
        public string RequestReasons { get; set; }
        public string SituationsOfDomesticViolence { get; set; }
        public string FamilyExpectations { get; set; }
        public string FamilyDiagnostic { get; set; }
        public string CaseStudyConclusion { get; set; }
        public string Recommendations { get; set; }
        public string VisualSupports { get; set; }
        public string Sketch { get; set; }
        public int LegalGuardianId { get; set; }
        public LegalGuardian LegalGuardian { get; set; }
        public int MinorId { get; set; }
        public Minor Minor { get; set; }
        public int PreviousFoundationId { get; set; }
        public PreviousFoundation PreviousFoundation { get; set; }
        public int FamilyHealthId { get; set; }
        public FamilyHealth FamilyHealth { get; set; }
        public int FamilyMembersId { get; set; }
        public FamilyMembers FamilyMembers { get; set; }
        public int SocioEconomicStudyId { get; set; }
        public SocioEconomicStudy SocioEconomicStudy { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
        public int EconomicSituationId { get; set; }
        public Economicsituation Economicsituation { get; set; }
        public int FamilyNutritionId { get; set; }
        public FamilyNutrition FamilyNutrition { get; set; }
        public int BenefitsProvidedId { get; set; }
        public BenefitsProvided BenefitsProvided { get; set; }
        public int IngresosEgresosMensualesId { get; set; }
        public IngresosEgresosMensuales IngresosEgresosMensuales { get; set; }
    }
}