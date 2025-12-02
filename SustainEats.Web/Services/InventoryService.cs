using SustainEats.Shared.Models;
using SustainEats.Shared.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace SustainEats.Web.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly DatabaseService _dbService;

        public InventoryService(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<PantryItem>> GetPantryItemsAsync(int userId)
        {
            var items = new List<PantryItem>();
            using var connection = _dbService.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.Id, p.Quantity, p.ExpiryDate,
                       i.Id, i.Name, i.BaseUnit, i.Protein, i.Fat, i.Carbs
                FROM PantryItems p
                INNER JOIN IngredientDefinitions i ON p.IngredientDefinitionId = i.Id
                WHERE p.UserId = $userId";
            command.Parameters.AddWithValue("$userId", userId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                items.Add(new PantryItem
                {
                    Id = reader.GetInt32(0),
                    Quantity = reader.GetDecimal(1),
                    ExpiryDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                    IngredientDefinitionId = reader.GetInt32(3),
                    IngredientDefinition = new IngredientDefinition
                    {
                        Id = reader.GetInt32(3),
                        Name = reader.GetString(4),
                        BaseUnit = (MeasureUnit)reader.GetInt32(5),
                        Protein = reader.GetDecimal(6),
                        Fat = reader.GetDecimal(7),
                        Carbs = reader.GetDecimal(8)
                    }
                });
            }
            return items;
        }

        public async Task AddPantryItemAsync(PantryItem item)
        {
            await Task.CompletedTask;
        }

        public async Task UpdatePantryItemAsync(PantryItem item)
        {
            await Task.CompletedTask;
        }

        public async Task RemovePantryItemAsync(int itemId)
        {
            await Task.CompletedTask;
        }
    }
}
