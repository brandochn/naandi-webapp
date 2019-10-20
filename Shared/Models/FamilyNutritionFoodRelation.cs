namespace Naandi.Shared.Models
{
    public class FamilyNutritionFoodRelation
    {
        public int Id { get; set; }
        public int FamilyNutritionId { get; set; }
        public int FoodId { get; set; }
        public int FrequencyId { get; set; }
    }
}