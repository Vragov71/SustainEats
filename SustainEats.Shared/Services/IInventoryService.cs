using System.Collections.Generic;
using System.Threading.Tasks;
using SustainEats.Shared.Models;

namespace SustainEats.Shared.Services
{
    public interface IInventoryService
    {
        Task<List<PantryItem>> GetPantryItemsAsync(int userId);
        Task AddPantryItemAsync(PantryItem item);
        Task UpdatePantryItemAsync(PantryItem item);
        Task RemovePantryItemAsync(int itemId);
    }
}