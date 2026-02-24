using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ProjPlatinaSteam.Controllers
{
    public class AuthController : Controller
    {
        //"a linha abaixo q faz o /login do program.cs vira pra ca"
        [Route("login")]
        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Steam");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
