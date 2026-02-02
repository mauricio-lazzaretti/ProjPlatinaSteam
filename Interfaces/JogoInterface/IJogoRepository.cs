using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.JogoInterface
{
    public interface IJogoRepository
    {
        Task<Jogo?> ObterPorSteamAppIdAsync(long steamAppId);
        Task AdicionarAsync(Jogo jogo);
        Task SaveAsync();
       void Update(Jogo jogo);
    }
}
