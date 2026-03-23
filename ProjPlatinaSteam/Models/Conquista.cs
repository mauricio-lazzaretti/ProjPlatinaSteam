using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjPlatinaSteam.Models
{
    public class Conquista
    {
        [Key] public int Id { get; set; }
        public string apiNome { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public bool conquistada { get; set; }
        public DateTime? unlockTime { get; set; }
        public bool ehDLC { get; set; } = false;

        public int JogoId { get; set; } //foreign key

        [JsonIgnore]
        public Jogo jogos { get; set; }

    }
}
