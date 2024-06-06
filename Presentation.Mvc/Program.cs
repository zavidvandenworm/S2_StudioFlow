using System.Data;
using System.Reflection;
using Application;
using InfrastructureDapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;
using dotenv.net;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddApplication();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddInfra();

builder.Services.AddScoped<IDbConnection>((sp) => new MySqlConnection(
    "User=studioflow;Database=studioflow;Password=studioflow;Server=localhost;Port=3307"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();