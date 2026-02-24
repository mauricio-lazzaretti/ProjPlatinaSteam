using Microsoft.AspNetCore.Authentication;
using ProjPlatinaSteam.Interfaces.UsuarioInterface;
using System.Security.Claims;

namespace ProjPlatinaSteam.Security
{
    public class SteamAuthEventsHandler
    {
        public static async Task OnTicketReceivedAsync(TicketReceivedContext context)
        {
            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
            var steamIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (steamIdClaim != null)
            {
                var usuarioService = context.HttpContext.RequestServices.GetRequiredService<IUsuarioService>();

                string steamIdLimpo = steamIdClaim.Value.Split('/').LastOrDefault();

                var usuario = await usuarioService.ObterUsuarioPorSteamId(steamIdLimpo);

                if (usuario != null && usuario.IsAdmin)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }
            }
        }
    }
}
