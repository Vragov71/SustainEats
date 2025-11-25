namespace SustainEats.Shared.Models
{
    public class IngredientDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Protein { get; set; } // per 100g/ml
        public decimal Fat { get; set; }     // per 100g/ml
        public decimal Carbs { get; set; }   // per 100g/ml
        public MeasureUnit BaseUnit { get; set; }
    }
}