using System.Collections.Generic;

namespace SustainEats.Shared.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int PreparationTime { get; set; } // in minutes

        // Cached macro values
        public decimal TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalFat { get; set; }
        public decimal TotalCarbs { get; set; }

        public ICollection<RecipeIngredient> Ingredients { get; set; }
    }
}