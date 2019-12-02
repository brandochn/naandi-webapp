namespace Naandi.Shared.Models
{
    public class FamilyNutrition
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public string FoodAllergy { get; set; }
        public FamilyNutritionFoodRelation[] FamilyNutritionFoodRelation {get; set;}
    }
}