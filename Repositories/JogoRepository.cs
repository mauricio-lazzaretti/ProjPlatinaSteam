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
                .FirstOrDefaultAsync(j => j.AppId == steamAppId && j.UsuarioSteamId == usuarioId);
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

        public async Task<int> ObterNumeroDeJogosBD(int UsuarioId)
        {
            return await _context.Jogos.CountAsync(j => j.UsuarioSteamId == UsuarioId);
        }

        public async Task<List<Jogo>> ObterJogosDoUserBD(int UsuarioId)
        {
            return await _context.Jogos.Where(j => j.UsuarioSteamId == UsuarioId).ToListAsync();
        }

        public async Task<List<Jogo>> ObterJogosOrdenadosDoUserBD(int UsuarioId, string ordem)
        {
            if(ordem == "nome_desc")
            {
                return await _context.Jogos.OrderByDescending(j => j.name).Where(j => j.UsuarioSteamId == UsuarioId).ToListAsync();
            }
            if (ordem == "nome_asc")
            {
                return await _context.Jogos.OrderBy(j => j.name).Where(j => j.UsuarioSteamId == UsuarioId).ToListAsync();
            }

            if(ordem == "horas_desc")
            {
                return await _context.Jogos.OrderByDescending(j => j.gameHours).Where(j => j.UsuarioSteamId == UsuarioId).ToListAsync();
            }

            if (ordem == "horas_asc")
            {
                return await _context.Jogos.OrderBy(j => j.gameHours).Where(j => j.UsuarioSteamId == UsuarioId).ToListAsync();
            }

            return await _context.Jogos.Where(j => j.UsuarioSteamId == UsuarioId).ToListAsync();
        }
    }
}
