namespace Krastev_It_API.Models.Identity
{
    public class LoginResponseModel
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public bool IsAdmin { get; set; }
    }
}
