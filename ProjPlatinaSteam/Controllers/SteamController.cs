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

        //private async Task<UsuarioSteam> Usuario(string steamId) //helper
        //{
        //    if (string.IsNullOrEmpty(steamId))
        //        return null;
        //   var usuario = await _usuarioService.ObterUsuarioBD(steamId);
        //    if(usuario == null)
        //    { 
        //        return usuario = await _usuarioService.ObterUsuarioAPI(steamId);           
        //    }
        //    else
        //        return usuario;
        //}

        public async Task<IActionResult> Ordem(string steamId, string ordem)
        {
            if (string.IsNullOrEmpty(steamId))
                return RedirectToAction("Index");

            var usuario = await _usuarioService.ObterUsuarioBD(steamId); ;

            var jogos = await _jogoService.ObterJogosOrdenadosDoUsuario(usuario.Id, ordem);

            var viewModel = new JogosViewModel
            {
                SteamId = steamId,
                Jogos = jogos
            };

            return View("Jogos", viewModel);
        }

        public async Task<IActionResult> Jogos(string steamId)
        {
            if (string.IsNullOrEmpty(steamId))
                return RedirectToAction("Index");

            var usuario = await _usuarioService.ObterUsuarioBD(steamId);
            List<Jogo> jogosUsuario;
            if (usuario != null)
            {
                jogosUsuario = await _jogoService.ObterJogosDoUsuario(steamId, usuario.Id); //ajustar aq para prever situacoes de horas e quantidade de jogos diferentes desde o ultimo login
            }
            else
            {
                usuario = await _usuarioService.ObterUsuarioAPI(steamId);
                jogosUsuario = await _jogoService.ObterJogosPrimeiroAcesso(steamId, usuario.Id); // em tese, e isso, mas precisa testar
            }
            //usar GetRecentlyPlayedGames para fazer a atualizacao das conquistas da barrinha de progresso q esta por vir

            var viewModel = new JogosViewModel
            {
                SteamId = steamId,
                Jogos = jogosUsuario
            };

            return View(viewModel);
        }

        //public async Task<IActionResult> OrdenarJogos(string steamId)
        //{
            
        //}

        public async Task<IActionResult> Conquistas(string steamId, string AppIdSteam, string jogoIdBanco)
        {
            if (string.IsNullOrEmpty(steamId) || string.IsNullOrEmpty(AppIdSteam))
                return RedirectToAction("Index");

            int.TryParse(jogoIdBanco, out int jogoIdInt);

            //var usuario = await Usuario(steamId);

            var conquistas = await _conquistaService.TrazerConquistasDoBD(jogoIdInt);

            return View(conquistas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
