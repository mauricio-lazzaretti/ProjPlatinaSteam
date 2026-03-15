using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.ConquistaInterface;
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
        private readonly IConquistaService _conquistaService;
        //private readonly IUsuarioRepository _usuarioRepository;

        public JogoService(ISteamApiService steamApiService, IJogoRepository jogoRepository, IConquistaService conquistaService)
        {
            _steamApiService = steamApiService;
            _jogoRepository = jogoRepository;
            _conquistaService = conquistaService;
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

            //var qtdJogosBanco = await _jogoRepository.ObterNumeroDeJogosBD(usuarioId); //conta quantos jogos o usuário tem no banco
            //var jogosUsuarioBD = await _jogoRepository.ObterJogosDoUserBD(usuarioId);// pega os dados dos jogos cad no banco antes de atualizar, para ser possivel ter acesso as horas de jogo

           // if (jogosSteam.Count != jogosUsuarioBD.Count) //logica para checar se o número de jogos do usuário mudou, caso tenha mudado, chama a função de sincronização
                await SincronizarJogosAsync(jogosSteam, usuarioId, steamId);

            var jogosUsuarioBD = await _jogoRepository.ObterJogosDoUserBD(usuarioId); //extraindo os dados do banco para retornar, mais seguro pois a steam pode cair

            //var primeiroJogo = jogosUsuarioBD.Where(p => p.AppId == 730).FirstOrDefault();
           // Console.WriteLine($"Jogo: {primeiroJogo?.name} - Conquistas carregadas: {primeiroJogo?.conquistas?.Count ?? 0}");

            return jogosUsuarioBD;
        }

        public async Task SincronizarJogosAsync(List<Jogo> jogosSteam, int usuarioId, string steamId)
        {
            foreach (var jogo in jogosSteam)
            {
                long appId = jogo.AppId;
                //int ap = jogo.id;

                var existente = await _jogoRepository.ObterPorSteamAppIdEUserAsync(appId, usuarioId);

                string StrgAppId = jogo.AppId.ToString();
                if (existente == null)
                {
                    await _jogoRepository.AdicionarAsync(jogo);
                    await _jogoRepository.SaveAsync();
                    await _conquistaService.ObterConquistasDoJogo(steamId, StrgAppId, jogo.id);
                }
                else
                {
                    //existente.name = jogo.name;
                    if (existente.gameHours != jogo.gameHours)
                    {
                        existente.gameHours = jogo.gameHours;
                        _jogoRepository.Update(existente);
                        await _jogoRepository.SaveAsync();
                        await _conquistaService.ObterConquistasDoJogo(steamId, StrgAppId, existente.id);
                    }
                }
            }

            await _jogoRepository.SaveAsync();
        }

        public async Task<List<Jogo>> ObterJogosOrdenadosDoUsuario(int usuarioId, string ordem)
        {
            return await _jogoRepository.ObterJogosOrdenadosDoUserBD(usuarioId, ordem);
        }

        public async Task<List<Jogo>> ObterJogosPrimeiroAcesso(string steamId, int usuarioId)
        {
            var jogosSteam = await _steamApiService.GetUserGames(steamId);

            foreach (var jogo in jogosSteam)
            {
                jogo.UsuarioSteamId = usuarioId;
            }

            await SincronizarJogosAsync(jogosSteam, usuarioId, steamId);
            var jogosUsuarioBD = await _jogoRepository.ObterJogosDoUserBD(usuarioId);

            return jogosUsuarioBD;
        }
    }
}
