using Microsoft.AspNetCore.Mvc;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using ProjPlatinaSteam.Interfaces.ConquistaInterface;
using ProjPlatinaSteam.Models;
using ProjPlatinaSteam.Models.ViewModels;

namespace ProjPlatinaSteam.Controllers
{
    public class SteamController : Controller
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IJogoService _jogoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IConquistaService _conquistaService;

        public SteamController(ISteamApiService steamApiService, IJogoService jogoService, IUsuarioService usuarioService, IConquistaService conquistaService)
        {
            _steamApiService = steamApiService;
            _jogoService = jogoService;
            _usuarioService = usuarioService;
            _conquistaService = conquistaService;
            _conquistaService = conquistaService;
        }

        [HttpGet("api/perfil/{steamId}")]
        public async Task<IActionResult> ObterPerfil(string steamId)
        {
            if (string.IsNullOrEmpty(steamId))
                return BadRequest("SteamId não informado");

            // usuario acaba tendo a lista de jogos, que por sua vez tem a lista de conquistas, um json completo
            var usuario = await _usuarioService.ObterOPerfilCompleto(steamId);
            if (usuario == null)
                return NotFound();

            return Ok(usuario); 
        }

        public async Task<IActionResult> Conquistas(string AppIdSteam, string jogoIdBanco)
        {
            if (string.IsNullOrEmpty(AppIdSteam))
                return RedirectToAction("Index");

            int.TryParse(jogoIdBanco, out int jogoIdInt);

            //var usuario = await Usuario(steamId);

            var conquistas = await _conquistaService.TrazerConquistasDoBD(jogoIdInt);

            return View(conquistas);
        }

        public IActionResult Index()
        {
            return Ok();
        }
    }
}
