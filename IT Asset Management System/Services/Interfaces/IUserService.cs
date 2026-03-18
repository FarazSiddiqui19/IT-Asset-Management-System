using System.Threading.Tasks;
using IT_Asset_Management_System.DTOs.User;
using IT_Asset_Management_System.Entities;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> AddUserAsync(CreateUserDto dto);
        Task<UserDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UpdateUserDto dto);
        Task DeactivateAsync(Guid id);
       
    }
}
