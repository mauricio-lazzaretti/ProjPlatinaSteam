using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.JogoInterface;
using ProjPlatinaSteam.Models;
//using ProjPlatinaSteam.Interfaces.UsuarioInterface;
//using ProjPlatinaSteam.Repositories;

namespace ProjPlatinaSteam.Services.JogoService
{
    public class JogoService : IJogoService
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IJogoRepository _jogoRepository;
        //private readonly IUsuarioRepository _usuarioRepository;

        public JogoService(ISteamApiService steamApiService, IJogoRepository jogoRepository)
        {
            _steamApiService = steamApiService;
            _jogoRepository = jogoRepository;
           // _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Jogo>> ObterJogosDoUsuario(string steamId, int usuarioId)
        {

            var jogosSteam = await _steamApiService.GetUserGames(steamId);

            if (jogosSteam == null || jogosSteam.Count == 0)
                return new List<Jogo>();

            foreach (var jogo in jogosSteam)
            {
                jogo.UsuarioSteamId = usuarioId;
            }

            var qtdJogosBanco = await _jogoRepository.ObterNumeroDeJogosBD(usuarioId); //conta quantos jogos o usuário tem no banco

            if (jogosSteam.Count != qtdJogosBanco) //logica para checar se o número de jogos do usuário mudou, caso tenha mudado, chama a função de sincronização
                await SincronizarJogosAsync(jogosSteam, usuarioId);

            var jogosUsuarioBD = await _jogoRepository.ObterJogosDoUserBD(usuarioId); //extraindo os dados do banco para retornar, mais seguro pois a steam pode cair

            return jogosUsuarioBD;
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

        public async Task<List<Jogo>> ObterJogosOrdenadosDoUsuario(int usuarioId, string ordem)
        {
            return await _jogoRepository.ObterJogosOrdenadosDoUserBD(usuarioId, ordem);
        }
    }
}
