using Microsoft.AspNetCore.Mvc;
using ProjPlatinaSteam.Interfaces;
using ProjPlatinaSteam.Models.ViewModels;

namespace ProjPlatinaSteam.Controllers
{
    public class SteamController : Controller
    {
        private readonly ISteamApiService _steamApiService;
        private readonly IJogoService _jogoService;

        public SteamController(ISteamApiService steamApiService, IJogoService jogoService)
        {
            _steamApiService = steamApiService;
            _jogoService = jogoService;
        }

        public async Task<IActionResult> Jogos(string steamId)
        {
            if (string.IsNullOrEmpty(steamId))
                return RedirectToAction("Index");

            var jogos = await _jogoService.ObterJogosDoUsuario(steamId);

            var viewModel = new JogosViewModel
            {
                SteamId = steamId,
                Jogos = jogos
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Conquistas(string steamId, string gameId)
        {
            if (string.IsNullOrEmpty(steamId) || string.IsNullOrEmpty(gameId))
                return RedirectToAction("Index");

            var conquistas = await _steamApiService.GetUserAchievements(steamId, gameId);

            return View(conquistas);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
