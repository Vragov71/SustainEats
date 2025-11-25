using Microsoft.EntityFrameworkCore;
using SustainEats.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SustainEats.Shared.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PantryItem>> GetPantryItemsAsync(int userId)
        {
            return await _context.PantryItems
                .Where(p => p.UserId == userId)
                .Include(p => p.IngredientDefinition)
                .ToListAsync();
        }

        public async Task AddPantryItemAsync(PantryItem item)
        {
            _context.PantryItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePantryItemAsync(PantryItem item)
        {
            _context.PantryItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePantryItemAsync(int itemId)
        {
            var item = await _context.PantryItems.FindAsync(itemId);
            if (item != null)
            {
                _context.PantryItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}