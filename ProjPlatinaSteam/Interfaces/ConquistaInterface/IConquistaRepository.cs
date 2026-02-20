using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.ConquistaInterface
{
    public interface IConquistaRepository
    {
        Task<List<Conquista>> ObterPorJogoId(int jogoIdBanco);
        Task AdicionarAsync(Conquista conquista);
        void Update(Conquista conquista);
        Task SaveAsync();

    }
}
