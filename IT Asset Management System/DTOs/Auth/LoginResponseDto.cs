using IT_Asset_Management_System.Entities.Enums;
using System;

namespace IT_Asset_Management_System.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;

        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public UserRole Role { get; set; }


        public bool IsActive { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
