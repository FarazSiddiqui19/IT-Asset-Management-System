using System.Threading.Tasks;
using IT_Asset_Management_System.DTOs.Auth;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    }
}
