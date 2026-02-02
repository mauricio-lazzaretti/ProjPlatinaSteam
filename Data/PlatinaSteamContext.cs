using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Data
{
    public class PlatinaSteamContext : DbContext
    {
        PlatinaSteamContext() { }

        public PlatinaSteamContext(DbContextOptions<PlatinaSteamContext> options) 
            : base(options)
        {
        }

        public DbSet<UsuarioSteam> UsuariosSteam { get; set; } = null!;
        public DbSet<Conquista> Conquistas { get; set; } = null!;
        public DbSet<Jogo> Jogos { get; set; } = null!;
    }
}
