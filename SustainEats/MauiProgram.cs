using Microsoft.Extensions.Logging;
using SustainEats.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace SustainEats;

// Dummy services for MAUI to compile
public class MauiAuthService : IAuthService {
    public Task<string> Login(SustainEats.Shared.Models.LoginModel model) => Task.FromResult<string>(null);
    public Task<bool> Register(SustainEats.Shared.Models.RegisterModel model) => Task.FromResult(false);
    public Task Logout() => Task.CompletedTask;
    public string GetUsername() => null;
}
public class MauiInventoryService : IInventoryService {
    public Task<System.Collections.Generic.List<SustainEats.Shared.Models.PantryItem>> GetPantryItemsAsync(int userId) => Task.FromResult(new System.Collections.Generic.List<SustainEats.Shared.Models.PantryItem>());
    public Task AddPantryItemAsync(SustainEats.Shared.Models.PantryItem item) => Task.CompletedTask;
    public Task UpdatePantryItemAsync(SustainEats.Shared.Models.PantryItem item) => Task.CompletedTask;
    public Task RemovePantryItemAsync(int itemId) => Task.CompletedTask;
}
public class MauiRecipeService : IRecipeService {
    public Task<System.Collections.Generic.List<SustainEats.Shared.Models.Recipe>> GetAllRecipesAsync() => Task.FromResult(new System.Collections.Generic.List<SustainEats.Shared.Models.Recipe>());
    public Task<SustainEats.Shared.Models.Recipe> GetRecipeByIdAsync(int recipeId) => Task.FromResult<SustainEats.Shared.Models.Recipe>(null);
    public Task AddRecipeAsync(SustainEats.Shared.Models.Recipe recipe) => Task.CompletedTask;
    public Task<System.Collections.Generic.List<SustainEats.Shared.Models.IngredientDefinition>> GetAvailableIngredientsAsync() => Task.FromResult(new System.Collections.Generic.List<SustainEats.Shared.Models.IngredientDefinition>());
}
public class MauiMacroCalculatorService : IMacroCalculatorService {
    public void CalculateAndSetMacros(SustainEats.Shared.Models.Recipe recipe) { }
}


public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();
        
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        builder.Services.AddAuthorizationCore();
        
        // Register DUMMY services for MAUI
        builder.Services.AddSingleton<DbPathService>();
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddScoped<IAuthService, MauiAuthService>();
        builder.Services.AddScoped<IInventoryService, MauiInventoryService>();
        builder.Services.AddScoped<IRecipeService, MauiRecipeService>();
        builder.Services.AddScoped<IMacroCalculatorService, MauiMacroCalculatorService>();
        builder.Services.AddScoped<CustomAuthStateProvider>();

        return builder.Build();
    }
}
