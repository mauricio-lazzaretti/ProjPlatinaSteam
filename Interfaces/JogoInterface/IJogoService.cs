using ProjPlatinaSteam.Models;
namespace ProjPlatinaSteam.Interfaces
{
    public interface IJogoService
    {
        Task<List<Jogo>> ObterJogosDoUsuario(string steamId);
        Task SincronizarJogosAsync(List<Jogo> jogosSteam);

    }
}
