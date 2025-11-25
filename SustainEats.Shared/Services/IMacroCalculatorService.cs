using SustainEats.Shared.Models;

namespace SustainEats.Shared.Services
{
    public interface IMacroCalculatorService
    {
        void CalculateAndSetMacros(Recipe recipe);
    }
}