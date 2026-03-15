using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.ConquistaInterface
{
    public interface IConquistaService
    {
        Task<List<Conquista>> ObterConquistasDoJogo(string steamId, string AppIdSteam, int jogoIdBanco);
        Task SincronizarConquistasAsync(List<Conquista> conquistasSteam, int jogoId);
        Task<List<Conquista>> TrazerConquistasDoBD(int jogoIdBanco);
    }
}
