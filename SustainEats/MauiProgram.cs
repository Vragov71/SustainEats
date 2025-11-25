using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SustainEats.Shared;
using SustainEats.Shared.Models;
using SustainEats.Shared.Services;

namespace SustainEats;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        // 1. Database Path
        builder.Services.AddSingleton<DbPathService>();
        var dbPath = new DbPathService().GetDbPath();

        // 2. DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        // 3. Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IInventoryService, InventoryService>();
        builder.Services.AddScoped<IMacroCalculatorService, MacroCalculatorService>();
        
        // 4. HttpClient for AuthService
        builder.Services.AddScoped<HttpClient>();

        // Add device-specific services used by the SustainEats.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Ensure database is created and migrated
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.Migrate();
            
            // Seed data
            if (!dbContext.Categories.Any())
            {
                // Main categories
                var appetizers = new Category { Name = "Предястия" };
                var salads = new Category { Name = "Салати" };
                var soups = new Category { Name = "Супи" };
                var mainCourses = new Category { Name = "Основни ястия" };
                var burgers = new Category { Name = "Бургери" };
                var sideDishes = new Category { Name = "Гарнитури" };
                var spreads = new Category { Name = "Разядки" };
                var breads = new Category { Name = "Хлебни изделия" };
                var desserts = new Category { Name = "Десерти" };

                dbContext.Categories.AddRange(appetizers, salads, soups, mainCourses, burgers, sideDishes, spreads, breads, desserts);
                dbContext.SaveChanges();

                // Sub-categories for main courses
                var meatDishes = new Category { Name = "Месни", ParentId = mainCourses.Id };
                var meatlessDishes = new Category { Name = "Безмесни", ParentId = mainCourses.Id };

                dbContext.Categories.AddRange(meatDishes, meatlessDishes);
                dbContext.SaveChanges();
            }
        }

        return app;
    }
}

public class FormFactor : IFormFactor
{
    // Basic implementation for MAUI
    public string GetFormFactor() => "Mobile";
    public string GetPlatform() => DeviceInfo.Platform.ToString();
    public bool IsMobile() => true;
}
