using System.ComponentModel.DataAnnotations;

namespace Dorywcza.Models.Auth
{
    public class UserAuth
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
