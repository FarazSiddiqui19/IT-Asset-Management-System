using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public required string Email { get; set; } 

        [Required]
        public required string Password { get; set; }
    }
}
