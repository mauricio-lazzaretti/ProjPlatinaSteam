using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Interfaces.UsuarioInterface
{
    public interface IUsuarioService
    {
        Task<List<UsuarioSteam>> ObterDadosUsuario(string steamId);
        //Task SincronizarUsuarioAsync(List<UsuarioSteam> usuarioSteam);
    }
}
