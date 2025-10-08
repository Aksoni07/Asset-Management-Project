using AssetManagement.BusinessLogic.Services;
using AssetManagement.DataAccess;
using AssetManagement.DataAccess.Repositories;
using AssetManagement.UI.Components;
using Microsoft.EntityFrameworkCore;
using AssetManagement.UI.Data;
using Microsoft.AspNetCore.Components.Authorization; // Add this
using AssetManagement.UI.Auth; // Add this

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Auth services
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();


// 1. Configure DBContext from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Register Repositories and Services for Dependency Injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();


var app = builder.Build();

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
    .AddInteractiveServerRenderMode();

DataSeeder.Seed(app);
app.Run();