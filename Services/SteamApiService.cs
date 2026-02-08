using Microsoft.Extensions.Options;
using ProjPlatinaSteam.Models;
using ProjPlatinaSteam.Models.Settings;
using ProjPlatinaSteam.Models.Steam;
using ProjPlatinaSteam.Models.Steam.Achievements;
using ProjPlatinaSteam.Models.Steam.Schema;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.DTOs;
using System.Net.Http;
using System.Reflection;
using System.Runtime;
using System.Text.Json;
using Microsoft.Identity.Client;

namespace ProjPlatinaSteam.Services
{
    public class SteamApiService : ISteamApiService
    {
        private readonly HttpClient _httpClient;
        private readonly SteamSettings _settings;
        public SteamApiService(
            HttpClient httpClient,
            IOptions<SteamSettings> options
            )
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }
        public async Task<List<Jogo>> GetUserGames(string steamId)
        {
            var url =
                $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?" +
                $"key={_settings.ApiKey}&steamid={steamId}&include_appinfo=true&include_played_free_games=true";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<Jogo>();
            }

            var content = await response.Content.ReadAsStringAsync();

            var steamResponse = JsonSerializer.Deserialize<SteamOwnedGamesResponse>(content);

            List<SteamGame> steamGames = steamResponse?.response?.games ?? new List<SteamGame>();

            var jogos = steamGames.Select(g => new Jogo
            {
                id = g.appid,
                name = g.name,
                gameHours = g.playtime_forever / 60,
            }).ToList();

            Console.WriteLine(content);

            return jogos;
        }

        public async Task<List<Conquista>> GetUserAchievements(string steamId, string gameId)
        {
            var achievements = await GetPlayerAchievements(steamId, gameId);
            var schemaAchievements = await GetSchemaForGame(gameId);


            var conquistas = achievements.Select(a => new Conquista
            {
                nome = schemaAchievements.FirstOrDefault(s => s.name == a.apiname)?.displayName ?? string.Empty.ToString(),
                descricao = schemaAchievements.FirstOrDefault(s => s.name == a.apiname)?.description ?? string.Empty.ToString(),
                apiNome = a.apiname,
                conquistada = a.achieved == 1,
                unlockTime = a.unlockTime > 0
                ? DateTimeOffset.FromUnixTimeSeconds(a.unlockTime).DateTime
                : null
            }).ToList();

            return conquistas;
            // return new List<Conquista>(); ;// retorna null por enquanto, so para evitar linha vermelha
        }

        public async Task<List<SteamAchievement>> GetPlayerAchievements(string steamId, string gameId)
        {
            var url =
               $"https://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v0001/?" +
               $"key={_settings.ApiKey}&steamid={steamId}&appid={gameId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<SteamAchievement>();
            }

            var content = await response.Content.ReadAsStringAsync();

            var steamAchievementsResponse = JsonSerializer.Deserialize<SteamAchievementsResponse>(content);

            List<SteamAchievement> achievements = steamAchievementsResponse?.playerstats?.achievements ?? new List<SteamAchievement>();

            return achievements;
        }

        public async Task<List<SteamSchemaAchievement>> GetSchemaForGame(string gameId)
        {
            var url =
               $"https://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?" +
               $"key={_settings.ApiKey}&appid={gameId}&l=brazilian";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<SteamSchemaAchievement>();
            }

            var content = await response.Content.ReadAsStringAsync();

            var steamSchemaAchievementsResponse = JsonSerializer.Deserialize<SteamGameSchemaResponse>(content);

            List<SteamSchemaAchievement> schemaAchievements = steamSchemaAchievementsResponse?.game?.availableGameStats?.achievements ?? new List<SteamSchemaAchievement>();

            return schemaAchievements;
        }

        public async Task<UsuarioSteamDTO> GetUserData(string steamId)
        {
            var url =
               $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?" +
               $"key={_settings.ApiKey}&steamids={steamId}";

            var result = await _httpClient.GetFromJsonAsync<SteamUserResponse>(url);

            return result?.response?.players?.FirstOrDefault();
        }
    }
}
