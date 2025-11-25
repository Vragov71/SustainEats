namespace SustainEats.Shared.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngredientDefinitionId { get; set; }
        public IngredientDefinition IngredientDefinition { get; set; }

        public decimal QuantityNeeded { get; set; } // In base units (g/ml)
    }
}