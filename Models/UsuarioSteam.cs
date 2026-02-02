using System.ComponentModel.DataAnnotations;

namespace ProjPlatinaSteam.Models
{
    public class UsuarioSteam
    {
        [Key] public int Id { get; set; }
        public string steamId { get; set; }
        public string name { get; set; }
        public string avatarUrl { get; set; }
        public List<Jogo> jogos { get; set; }
    }
}
