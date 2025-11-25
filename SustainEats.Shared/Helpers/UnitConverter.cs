using SustainEats.Shared.Models;

namespace SustainEats.Shared.Helpers
{
    public static class UnitConverter
    {
        public static (decimal, MeasureUnit) ToBaseUnit(decimal value, MeasureUnit unit)
        {
            return unit switch
            {
                MeasureUnit.kg => (value * 1000, MeasureUnit.g),
                MeasureUnit.l => (value * 1000, MeasureUnit.ml),
                _ => (value, unit)
            };
        }
    }
}