namespace ProjPlatinaSteam.Models
{
    public class Conquista
    {
        //decidi usar os nomes de variaveis em ingles para padronizar com mercado

        public string apiNome { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public bool conquistada { get; set; }
        public DateTime? unlockTime { get; set; }
        public bool ehDLC { get; set; }

    }
}
