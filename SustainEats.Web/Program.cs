using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SustainEats.Shared;
using SustainEats.Shared.Models;
using SustainEats.Web.Components;
using SustainEats.Shared.Services;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

// --- AUTH SERVICES ---
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
// ---------------------

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Services are now in Shared project
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IMacroCalculatorService, MacroCalculatorService>();
builder.Services.AddScoped<HttpClient>(); 


// Add device-specific services used by the SustainEats.Shared project
builder.Services.AddSingleton<IFormFactor, WebFormFactor>(); 

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();

    if (!dbContext.Categories.Any())
    {
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

public class WebFormFactor : IFormFactor
{
    public string GetFormFactor() => "Web";
    public string GetPlatform() => "Web";
    public bool IsMobile() => false;
}
