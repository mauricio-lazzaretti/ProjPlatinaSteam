using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces
{
    public interface ISteamApiService
    {
        Task<List<Jogo>> GetUserGames(string steamId);
        Task<List<Conquista>> GetUserAchievements(string steamId, string gameId);

    }
}

