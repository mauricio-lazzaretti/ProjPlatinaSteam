using Microsoft.Extensions.Caching.Memory;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Models;

namespace ProjPlatinaSteam.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoService _jogoService;
        private readonly IMemoryCache _memoryCache;
        public UsuarioService(ISteamApiService steamApiService, IUsuarioRepository usuarioRepository, IJogoService jogoService, IMemoryCache memoryCache) 
        { 
            _steamApiService = steamApiService;
            _usuarioRepository = usuarioRepository;
            _jogoService = jogoService;
            _memoryCache = memoryCache;
        }
        //public Task<List<UsuarioSteam>> ObterDadosUsuario(string steamId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<UsuarioSteam?> ObterOPerfilCompleto(string steamId)
        {
            string cacheKey = "perfil_completo_{steamId}";

            if (_memoryCache.TryGetValue(cacheKey, out UsuarioSteam? usuarioNoCache))
                return usuarioNoCache;

            var usuario = await ObterUsuarioBD(steamId);
            List<Jogo> jogosUsuario;
            if (usuario != null)
            {
                jogosUsuario = await _jogoService.ObterJogosDoUsuario(steamId, usuario.Id); //Busca os jogos diretamente do BD, exceto pelos jogos novos
            }
            else
            {
                usuario = await ObterUsuarioAPI(steamId);
                if(usuario == null)
                    return null;

                jogosUsuario = await _jogoService.ObterJogosPrimeiroAcesso(steamId, usuario.Id); // Primeiro acesso do user, busca todos os dados da Api e leva para o BD         
            }

            if (usuario != null)
            {
                var cacheConfig = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _memoryCache.Set(cacheKey, usuario, cacheConfig);
            }

            return usuario; // usuario acaba tendo a lista de jogos, que por sua vez tem a lista de conquistas, um json completo
        }

        public async Task<UsuarioSteam?> ObterUsuarioBD(string steamId)
        {
            if (!long.TryParse(steamId, out long steamIdLong))
            {
                return await Task.FromResult<UsuarioSteam>(null);
            }
            var usuario = await _usuarioRepository.ObterPorSteamAppIdAsync(steamIdLong);
          
            return usuario;
        }

        public async Task<UsuarioSteam?> ObterUsuarioAPI(string steamId)
        {  
            UsuarioSteam usuario = null;
            var userdto = await _steamApiService.GetUserData(steamId);
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
