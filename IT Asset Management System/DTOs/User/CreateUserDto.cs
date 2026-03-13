using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; } 

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; } 

        [Required]
        [MinLength(8)]
        public required string Password { get; set; } 

        [Required]
        public UserRole Role { get; set; }
    }
}
