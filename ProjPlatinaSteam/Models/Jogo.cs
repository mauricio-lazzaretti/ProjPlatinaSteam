using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjPlatinaSteam.Models
{
    public class Jogo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int AppId { get; set; }

        public string name { get; set; }

        public int gameHours { get; set; }

        public List<Conquista> conquistas { get; set; }

        public int? UsuarioSteamId { get; set; } //foreign key

        [JsonIgnore]
        public UsuarioSteam UsuarioSteam { get; set; }

    }
}
