using Microsoft.AspNetCore.Mvc;
using ProjPlatinaSteam.Models;
using ProjPlatinaSteam.Models.ViewModels;
using ProjPlatinaSteam.Services;

namespace ProjPlatinaSteam.Controllers
{
    public class SteamController : Controller
    {
        private readonly SteamApiService _steamApiService;

        public SteamController(SteamApiService steamApiService)
        {
            _steamApiService = steamApiService;
        }

        //public async Task<IActionResult> Teste()
        //{
        //    var jogos = await _steamApiService.GetUserGames("76561199564370952");
        //    return Json(jogos);
        //}

        public async Task<IActionResult> Jogos(string steamId)
        {
            if(string.IsNullOrEmpty(steamId))
            {
                return RedirectToAction("Index");
            }

            var jogos = await _steamApiService.GetUserGames(steamId);

            var viewModel = new JogosViewModel
            {
                SteamId = steamId,
                Jogos = jogos
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Conquistas( string steamId, string gameId)
        {
            //metodo que deve carregar as conquistas de um jogo
            if(string.IsNullOrEmpty(steamId))
            {
                return RedirectToAction("Index");
            }

            var conquistas = await _steamApiService.GetUserAchievements(steamId, gameId);

            return View(conquistas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
