using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.User
{
    public class UpdateUserDto
    {

   
        [MinLength(3)]
        [MaxLength(20)]
        public string? Username { get; set; }

      
        [MinLength(8)]
        public string? Password { get; set; } 
    }
}
