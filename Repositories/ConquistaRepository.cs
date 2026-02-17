using Microsoft.EntityFrameworkCore;
using ProjPlatinaSteam.Data;
using ProjPlatinaSteam.Interfaces.ConquistaInterface;
using ProjPlatinaSteam.Models;
namespace ProjPlatinaSteam.Repositories
{
    public class ConquistaRepository : IConquistaRepository
    {
        private readonly PlatinaSteamContext _context;
        public ConquistaRepository(PlatinaSteamContext context) 
        {
            _context = context;
        }

        public async Task<List<Conquista>> ObterPorJogoId(int jogoId)
        {
            return await _context.Conquistas.Where(c => c.JogoId == jogoId).ToListAsync();
        }

        public async Task AdicionarAsync(Conquista conquista)
        {
            await _context.Conquistas.AddAsync(conquista);
        }

        public void Update(Conquista conquista)
        {
            _context.Conquistas.Update(conquista);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
