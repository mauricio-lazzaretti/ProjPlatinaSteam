namespace ProjPlatinaSteam.Models.Steam.Achievements
{
    public class PlayerStats
    {
        public string steamId { get; set; }
        public string gameName { get; set; }

        public List<SteamAchievement> achievements { get; set; }
    }
}
