using Microsoft.EntityFrameworkCore;
using ProjPlatinaSteam.Data;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.JogoInterface;
using ProjPlatinaSteam.Models.Settings;
using ProjPlatinaSteam.Repositories;
using ProjPlatinaSteam.Services;
using ProjPlatinaSteam.Services.JogoService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<SteamApiService>();

builder.Services.Configure<SteamSettings>(
    builder.Configuration.GetSection("Steam"));

builder.Services.AddDbContext<PlatinaSteamContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IJogoService, JogoService>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<ISteamApiService, SteamApiService>();


//Console.WriteLine($"Rodando na versão: {Environment.Version}");

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Steam}/{action=Index}/{id?}");

app.Run();
