using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.DTOs.Auth;
using System.Security.Claims;
using IT_Asset_Management_System.DTOs.User;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
   
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
     
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
      
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            var userDto = await _userService.AddUserAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = userDto.Id }, userDto);
        }

        private Guid GetRequestingUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string GetRequestingUserRole() => (
            User.FindFirstValue(ClaimTypes.Role)!);

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(Guid id)
        {
            if (GetRequestingUserRole() == UserRole.Employee.ToString() && GetRequestingUserId() != id)
            {
                return Forbid();
            }
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            if(GetRequestingUserId() !=id)
            {
                return Forbid();
            }
            await _userService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        
        public async Task<IActionResult> Delete(Guid id)
        {

            if (GetRequestingUserId() != id)
            {
                return Forbid();
            }

            await _userService.DeleteAsync(id);
            return NoContent();
        }

       
    }
}
