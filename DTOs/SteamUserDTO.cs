namespace ProjPlatinaSteam.DTOs
{
    public class  SteamUserResponse
    {
        public PlayerListContainer response { get; set; }
    }
    public class PlayerListContainer
    {
        public List<UsuarioSteamDTO> players { get; set; }
    }
    public class UsuarioSteamDTO
    {
        public string steamid { get; set; }
        public string personaname { get; set; }
        public string avatarfull { get; set; }
    }
}
