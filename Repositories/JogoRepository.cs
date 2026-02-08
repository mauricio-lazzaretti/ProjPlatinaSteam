using Microsoft.EntityFrameworkCore;
using ProjPlatinaSteam.Data;
using ProjPlatinaSteam.Interfaces.JogoInterface;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly PlatinaSteamContext _context;

        public JogoRepository(PlatinaSteamContext context)
        {
            _context = context;
        }

        public async Task<Jogo?> ObterPorSteamAppIdEUserAsync(long steamAppId, int usuarioId)
        {
            return await _context.Jogos
                .FirstOrDefaultAsync(j => j.id == steamAppId && j.UsuarioSteamId == usuarioId);
        }

        public async Task AdicionarAsync(Jogo jogo)
        {
            await _context.Jogos.AddAsync(jogo);
        }

        public void Update(Jogo jogo)
        {
            _context.Jogos.Update(jogo);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
