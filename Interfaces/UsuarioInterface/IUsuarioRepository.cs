using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.UsuarioInterface
{
    public interface IUsuarioRepository
    {
        Task<UsuarioSteam?> ObterPorSteamAppIdAsync(long steamAppId);
        Task AdicionarAsync(UsuarioSteam usuario);
        Task SaveAsync();
    }
}
