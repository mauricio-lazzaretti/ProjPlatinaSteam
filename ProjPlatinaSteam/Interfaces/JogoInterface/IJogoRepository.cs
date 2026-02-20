using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.JogoInterface
{
    public interface IJogoRepository
    {
        Task<int> ObterNumeroDeJogosBD(int UsuarioId); // Método para obter o número total de jogos de um usuário específico
        Task<List<Jogo>> ObterJogosDoUserBD(int UsuarioId); // Método para obter a lista de jogos de um usuário específico
        Task<List<Jogo>> ObterJogosOrdenadosDoUserBD(int UsuarioId, string ordem); // Método para obter a lista de jogos de um usuário específico ordenada
        Task<Jogo?> ObterPorSteamAppIdEUserAsync(long steamAppId, int usuarioId); // Método para obter um jogo específico com base no SteamAppId e no ID do usuário
        Task AdicionarAsync(Jogo jogo); // Método para adicionar um novo jogo ao banco de dados
        Task SaveAsync(); // Método para salvar as alterações no banco de dados
        void Update(Jogo jogo); // Método para atualizar um jogo existente
    }
}
