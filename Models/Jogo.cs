namespace ProjPlatinaSteam.Models
{
    public class Jogo
    {
        public string id { get; set; }

        public string name { get; set; }

        public int gameHours { get; set; }

        public List<Conquista> conquistas { get; set; }

    }
}
