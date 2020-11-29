using System.ComponentModel.DataAnnotations;

namespace Krastev_It_API.Models.Identity
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
