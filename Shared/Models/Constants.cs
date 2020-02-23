namespace Naandi.Shared.Models
{
    public static class Constants
    {
        public const string UNHANDLED_EXCEPTION_MESSAGE = "Unhandled exception has occurred, please check the error log for details";

        // Here are session Key Names for generic objects. Use UserSession class to create session Key Names for strongly typed objects
        public const string FamilyResearch_FamilyMembers_Table = nameof(FamilyResearch_FamilyMembers_Table);
        public const string FamilyResearch_BenefitsProvided_Table = nameof(FamilyResearch_BenefitsProvided_Table);
        public const string FamilyResearch_IngresosMensuales_Table = nameof(FamilyResearch_IngresosMensuales_Table);
        public const string FamilyResearch_EgresosMensuales_Table = nameof(FamilyResearch_EgresosMensuales_Table);
    }
}