using Microsoft.EntityFrameworkCore;
using SustainEats.Shared;
using SustainEats.Shared.Models;
using SustainEats.Web.Components;
using SustainEats.Shared.Services; // Changed from .Web.Services

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Services are now in Shared project
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IMacroCalculatorService, MacroCalculatorService>();
builder.Services.AddScoped<HttpClient>();


// Add device-specific services used by the SustainEats.Shared project
builder.Services.AddSingleton<IFormFactor, WebFormFactor>(); // Using a specific implementation for Web

var app = builder.Build();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Use migrations to update the database schema
    dbContext.Database.Migrate();

    if (!dbContext.Categories.Any())
    {
        // Seeding logic remains the same
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
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SustainEats.Shared._Imports).Assembly);

app.MapControllers();

app.Run();

// Specific implementation of IFormFactor for the Web project
public class WebFormFactor : IFormFactor
{
    public string GetFormFactor() => "Web";
    public string GetPlatform() => "Web";
    public bool IsMobile() => false;
}
