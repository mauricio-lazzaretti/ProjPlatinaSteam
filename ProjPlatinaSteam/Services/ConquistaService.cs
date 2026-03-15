using ProjPlatinaSteam.Models;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.ConquistaInterface;

namespace ProjPlatinaSteam.Services
{
    public class ConquistaService : IConquistaService
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IConquistaRepository _conquistaRepository;
        public ConquistaService(ISteamApiService steamApiService, IConquistaRepository conquistaRepository  )
        {
            _steamApiService = steamApiService;
            _conquistaRepository = conquistaRepository;
        }
        public async Task<List<Conquista>> ObterConquistasDoJogo(string steamId, string AppIdSteam, int jogoIdBanco)
        {
            var conquistasApi = await _steamApiService.GetUserAchievements(steamId, AppIdSteam, jogoIdBanco);

            await SincronizarConquistasAsync(conquistasApi, jogoIdBanco);

            var conquistasBanco = await _conquistaRepository.ObterPorJogoId(jogoIdBanco);

            return conquistasBanco;
        }

        public async Task SincronizarConquistasAsync(List<Conquista> conquistasSteam, int jogoIdBanco)
        {
            var conquistasNoBanco = await _conquistaRepository.ObterPorJogoId(jogoIdBanco); //dados "antigos"

            foreach (var conquistaApi in conquistasSteam)// dados frescos
            {

                var existente = conquistasNoBanco.FirstOrDefault(c => c.apiNome == conquistaApi.apiNome);

                if (existente == null)
                {
                    await _conquistaRepository.AdicionarAsync(conquistaApi);
                }
                else
                {
                    if (existente.conquistada == false && conquistaApi.conquistada == true) //compara dados antigos com frescos
                    {
                        existente.conquistada = true;//atualiza conquistada para conquistada
                        existente.unlockTime = conquistaApi.unlockTime;//pega a data q foi conquistada
                        _conquistaRepository.Update(existente);
                    }
                }
            }

            // Salva tudo de uma vez
            await _conquistaRepository.SaveAsync();
        }

        public async Task<List<Conquista>> TrazerConquistasDoBD(int jogoIdBanco)
        {
            return await _conquistaRepository.ObterPorJogoId(jogoIdBanco);
        }

    }
}
