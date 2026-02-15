using ProjPlatinaSteam.Models;
namespace ProjPlatinaSteam.Interfaces
{
    public interface IJogoService
    {
        Task<List<Jogo>> ObterJogosDoUsuario(string steamId, int usuarioId); // Método para obter os jogos do usuário a partir do Steam ID
        Task SincronizarJogosAsync(List<Jogo> jogosSteam, int usuarioId); // Método para sincronizar os jogos do usuário com os dados obtidos da API do Steam
        Task<List<Jogo>> ObterJogosOrdenadosDoUsuario(int usuarioId, string ordem); // Método para obter os jogos do usuário ordenados por um critério específico
                                                                                                    //   Task<List<Jogo>> OrdenarJogosDoUsuario(string steamId);

    }
}
