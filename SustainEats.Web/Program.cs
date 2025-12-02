using Microsoft.AspNetCore.Components.Authorization;
using SustainEats.Shared.Services;
using SustainEats.Web.Components;
using SustainEats.Web.Services; // Correct namespace
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddSingleton<DbPathService>(sp => new DbPathService("app.db"));
builder.Services.AddSingleton<DatabaseService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IMacroCalculatorService, MacroCalculatorService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<HttpClient>(); 

builder.Services.AddSingleton<IFormFactor, WebFormFactor>(); 

var app = builder.Build();

app.Services.GetRequiredService<DatabaseService>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddAdditionalAssemblies(typeof(SustainEats.Shared._Imports).Assembly);
app.MapControllers();

app.Run();

public class WebFormFactor : IFormFactor
{
    public string GetFormFactor() => "Web";
    public string GetPlatform() => "Web";
    public bool IsMobile() => false;
}
