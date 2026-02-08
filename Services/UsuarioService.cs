using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly SteamApiService _steamApiService;
        public UsuarioService(SteamApiService steamApiService) 
        { 
            _steamApiService = steamApiService;
        }
        public Task<List<UsuarioSteam>> ObterDadosUsuario(string steamId)
        {
            throw new NotImplementedException();
        }
    }
}
