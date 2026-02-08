using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.JogoInterface;
using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Models;
using ProjPlatinaSteam.Repositories;

namespace ProjPlatinaSteam.Services.JogoService
{
    public class JogoService : IJogoService
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IJogoRepository _jogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public JogoService(ISteamApiService steamApiService, IJogoRepository jogoRepository, IUsuarioRepository usuarioRepository)
        {
            _steamApiService = steamApiService;
            _jogoRepository = jogoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Jogo>> ObterJogosDoUsuario(string steamId)
        {
            if (!long.TryParse(steamId, out long steamIdLong))
            {
                return new List<Jogo>();
            }

            var usuario = await _usuarioRepository.ObterPorSteamAppIdAsync(steamIdLong);

            if (usuario == null)
            {
                var dto = await _steamApiService.GetUserData(steamId);

                if (dto != null)
                {
                    usuario = new UsuarioSteam
                    {
                        steamId = steamId, 
                        name = dto.personaname,
                        avatarUrl = dto.avatarfull
                    };

                    await _usuarioRepository.AdicionarAsync(usuario);
                    await _usuarioRepository.SaveAsync();
                }
            }

            var jogosSteam = await _steamApiService.GetUserGames(steamId);

            if (jogosSteam == null || jogosSteam.Count == 0)
                return new List<Jogo>();

            foreach (var jogo in jogosSteam)
            {
                jogo.UsuarioSteamId = usuario.Id;
            }

            await SincronizarJogosAsync(jogosSteam, usuario.Id);

            return jogosSteam;
        }

        public async Task SincronizarJogosAsync(List<Jogo> jogosSteam, int usuarioId)
        {
            foreach (var jogo in jogosSteam)
            {
               var appId = (long)jogo.id;

                var existente = await _jogoRepository.ObterPorSteamAppIdEUserAsync(appId, usuarioId);

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
