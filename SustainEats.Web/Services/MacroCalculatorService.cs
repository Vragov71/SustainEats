using SustainEats.Shared.Models;
using SustainEats.Shared.Services;
using System.Linq;

namespace SustainEats.Web.Services
{
    public class MacroCalculatorService : IMacroCalculatorService
    {
        // Basic calorie calculation constants
        private const decimal CALORIES_PER_GRAM_PROTEIN = 4;
        private const decimal CALORIES_PER_GRAM_CARBS = 4;
        private const decimal CALORIES_PER_GRAM_FAT = 9;

        public void CalculateAndSetMacros(Recipe recipe)
        {
            if (recipe.Ingredients == null || !recipe.Ingredients.Any())
            {
                recipe.TotalProtein = 0;
                recipe.TotalFat = 0;
                recipe.TotalCarbs = 0;
                recipe.TotalCalories = 0;
                return;
            }

            decimal totalProtein = 0;
            decimal totalFat = 0;
            decimal totalCarbs = 0;

            foreach (var ingredient in recipe.Ingredients)
            {
                // Macros are per 100g/ml, so we adjust based on the quantity needed
                var quantityFactor = ingredient.QuantityNeeded / 100m;
                totalProtein += ingredient.IngredientDefinition.Protein * quantityFactor;
                totalFat += ingredient.IngredientDefinition.Fat * quantityFactor;
                totalCarbs += ingredient.IngredientDefinition.Carbs * quantityFactor;
            }

            recipe.TotalProtein = totalProtein;
            recipe.TotalFat = totalFat;
            recipe.TotalCarbs = totalCarbs;
            recipe.TotalCalories = (totalProtein * CALORIES_PER_GRAM_PROTEIN) +
                                  (totalCarbs * CALORIES_PER_GRAM_CARBS) +
                                  (totalFat * CALORIES_PER_GRAM_FAT);
        }
    }
}