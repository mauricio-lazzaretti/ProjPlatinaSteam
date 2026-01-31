namespace ProjPlatinaSteam.Models
{
    public class UsuarioSteam
    {
        public string steamId { get; set; }
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public List<Jogo> jogos { get; set; }
    }
}
