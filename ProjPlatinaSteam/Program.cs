using AspNet.Security.OpenId;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProjPlatinaSteam.Data;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.ConquistaInterface;
using ProjPlatinaSteam.Interfaces.JogoInterface;
using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Models.Settings;
using ProjPlatinaSteam.Repositories;
using ProjPlatinaSteam.Security;
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
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IConquistaService, ConquistaService>();
builder.Services.AddScoped<IConquistaRepository, ConquistaRepository>();

//Console.WriteLine($"Rodando na versão: {Environment.Version}");

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Steam";
})
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/Home/Index";
    })
    .AddSteam(options =>
    {
        options.ApplicationKey = builder.Configuration["SteamSettings:ApiKey"];

        options.Events = new OpenIdAuthenticationEvents
        {
            OnTicketReceived = SteamAuthEventsHandler.OnTicketReceivedAsync
        };
    });

// Adiciona a política de acesso
builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaAppSteam", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer site
              .AllowAnyHeader()  // Permite qualquer cabeçalho
              .AllowAnyMethod(); // Permite GET, POST, etc.
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<PlatinaSteamContext>();
        context.Database.Migrate();
        Console.WriteLine("Base de dados pronta!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Aviso: A base de dados já existe ou está a ligar. Ignorando erro de arranque...");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("MinhaAppSteam");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Steam}/{action=Index}/{id?}");

app.Run();
