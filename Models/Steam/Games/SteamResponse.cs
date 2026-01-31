namespace ProjPlatinaSteam.Models.Steam
{
    public class SteamResponse
    {
        public int game_count { get; set; }

        public List<SteamGame> games { get; set; }
    }
}
