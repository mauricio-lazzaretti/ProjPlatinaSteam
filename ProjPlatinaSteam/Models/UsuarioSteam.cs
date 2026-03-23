using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjPlatinaSteam.Models
{
    public class UsuarioSteam
    {
        [Key] public int Id { get; set; }
        public string steamId { get; set; }

        public bool IsAdmin { get; set; } = false;
        public string name { get; set; }
        public string avatarUrl { get; set; }

        public List<Jogo> jogos { get; set; }
    }
}
