using System;

namespace SustainEats.Shared.Models
{
    public class PantryItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int IngredientDefinitionId { get; set; }
        public IngredientDefinition IngredientDefinition { get; set; }

        public decimal Quantity { get; set; } // Always in base units (g/ml)

        public DateTime? ExpiryDate { get; set; }
    }
}