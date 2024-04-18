using Application;
using Blazored.Toast;
using Infrastructure;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.SqlCommands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Presentation.Components;
using Presentation.Services;
using AuthenticationService = Presentation.Mvc.AuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredToast();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddSession(s =>
{
    
});

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<SqlConnectionFactory>();
builder.Services.AddScoped<ProjectCommands>();
builder.Services.AddScoped<UserCommands>();


var app = builder.Build();

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
    .AddInteractiveServerRenderMode();

app.Run();