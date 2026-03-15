using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly SteamApiService _steamApiService;
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(SteamApiService steamApiService, IUsuarioRepository usuarioRepository) 
        { 
            _steamApiService = steamApiService;
            _usuarioRepository = usuarioRepository;
        }
        //public Task<List<UsuarioSteam>> ObterDadosUsuario(string steamId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<UsuarioSteam> ObterUsuarioBD(string steamId)
        {
            if (!long.TryParse(steamId, out long steamIdLong))
            {
                return await Task.FromResult<UsuarioSteam>(null);
            }
            var usuario = await _usuarioRepository.ObterPorSteamAppIdAsync(steamIdLong);
          
            return usuario;
        }

        public async Task<UsuarioSteam> ObterUsuarioAPI(string steamId)
        {  
            var userdto = await _steamApiService.GetUserData(steamId);
            UsuarioSteam usuario = null;
            if (userdto != null)
            {
                usuario = new UsuarioSteam
                {
                    steamId = steamId,
                    name = userdto.personaname,
                    avatarUrl = userdto.avatarfull
                };

                await _usuarioRepository.AdicionarAsync(usuario);
                await _usuarioRepository.SaveAsync();
            }

            return usuario;
        }
    }
}
