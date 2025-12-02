using SustainEats.Shared.Models;
using SustainEats.Shared.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SustainEats.Web.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly DatabaseService _dbService;

        public RecipeService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            var recipes = new List<Recipe>();
            await Task.CompletedTask;
            return recipes;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            await Task.CompletedTask;
            return new Recipe { Title = "Not Implemented" };
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await Task.CompletedTask;
        }

        public async Task<List<IngredientDefinition>> GetAvailableIngredientsAsync()
        {
            var ingredients = new List<IngredientDefinition>();
            await Task.CompletedTask;
            return ingredients;
        }
    }
}
