using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.User;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }


        public static User ToEntity(this CreateUserDto dto,string PasswordHash)
        {
            return new User
            {
                Email = dto.Email,
                PasswordHash = PasswordHash,
                Username = dto.Username,
                Role = dto.Role
            };
        }
    }
}
