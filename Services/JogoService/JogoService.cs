using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.JogoInterface;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Services.JogoService
{
    public class JogoService : IJogoService
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IJogoRepository _jogoRepository;

        public JogoService(ISteamApiService steamApiService, IJogoRepository jogoRepository)
        {
            _steamApiService = steamApiService;
            _jogoRepository = jogoRepository;
        }

        public async Task<List<Jogo>> ObterJogosDoUsuario(string steamId)
        {
            var jogosSteam = await _steamApiService.GetUserGames(steamId);

            if (jogosSteam == null || jogosSteam.Count == 0)
                return new List<Jogo>();

            await SincronizarJogosAsync(jogosSteam);

            return jogosSteam;
        }

        public async Task SincronizarJogosAsync(List<Jogo> jogosSteam)
        {
            foreach (var jogo in jogosSteam)
            {
                if (!long.TryParse(jogo.id, out var appId))
                    continue;

                var existente = await _jogoRepository.ObterPorSteamAppIdAsync(appId);

                if (existente == null)
                {
                    await _jogoRepository.AdicionarAsync(jogo);
                }
                else
                {
                    existente.name = jogo.name;
                    existente.gameHours = jogo.gameHours;
                    _jogoRepository.Update(existente);
                }
            }

            // Salva tudo de uma vez
            await _jogoRepository.SaveAsync();
        }
    }
}
