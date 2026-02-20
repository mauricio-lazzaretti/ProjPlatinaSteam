
using Microsoft.EntityFrameworkCore;
using ProjPlatinaSteam.Data;
using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly PlatinaSteamContext _context;

        public UsuarioRepository(PlatinaSteamContext context)
        {
            _context = context;
        }

        public async Task<UsuarioSteam?> ObterPorSteamAppIdAsync(long steamAppId)
        {
            return await _context.UsuariosSteam.FirstOrDefaultAsync(u => u.steamId == steamAppId.ToString());
        }

        public async Task AdicionarAsync(UsuarioSteam usuario)
        {
            await _context.UsuariosSteam.AddAsync(usuario);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}