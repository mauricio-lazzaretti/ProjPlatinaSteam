using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.JogoInterface
{
    public interface IJogoRepository
    {
        Task<int> ObterNumeroDeJogosBD(int UsuarioId);
        Task<List<Jogo>> ObterJogosDoUserBD(int UsuarioId);
        Task<Jogo?> ObterPorSteamAppIdEUserAsync(long steamAppId, int usuarioId);
        Task AdicionarAsync(Jogo jogo);
        Task SaveAsync();
       void Update(Jogo jogo);
    }
}
