using AssetManagement.BusinessLogic.Services;
using AssetManagement.DataAccess;
using AssetManagement.DataAccess.Repositories;
using AssetManagement.UI.Components;
using Microsoft.EntityFrameworkCore;
using AssetManagement.UI.Data;
using Microsoft.AspNetCore.Components.Authorization;
using AssetManagement.UI.Auth;
using AssetManagement.UI.Services; 

var builder = WebApplication.CreateBuilder(args); // Web server setup, Load config from appsetting

// Add services to the container : configures the application to run in Blazor Server mode
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); //Blazor server service on:  server will manage the UI

// Add Auth services
builder.Services.AddCascadingAuthenticationState(); // passes the user's current login status down
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();

// 1. Configure DBContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Register Repositories and Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<CsvExportService>();


//MiddleWare Pipeline
var app = builder.Build(); // creates the actual web application object

// Configure the HTTP request pipeline : howv errors are handled in a production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}// shows a user-friendly error page 


//security &  utility middlewares
app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS.
app.UseStaticFiles();      // Allows serving files from the wwwroot folder (CSS, JS, images).
app.UseAntiforgery();        // Adds protection against cross-site request forgery attacks.

app.MapRazorComponents<App>() // handle all requests , route them
    .AddInteractiveServerRenderMode();// real-time connection

DataSeeder.Seed(app); // populate the database with sample data before the application starts listening for requests.
app.Run();