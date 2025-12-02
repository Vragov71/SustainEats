using SustainEats.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SustainEats.Shared.Services
{
    public interface IRecipeService
    {
        Task<List<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task AddRecipeAsync(Recipe recipe);
        Task<List<IngredientDefinition>> GetAvailableIngredientsAsync();
    }
}
