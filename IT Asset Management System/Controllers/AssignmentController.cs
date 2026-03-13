using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.DTOs.Assignment;

namespace IT_Asset_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        private Guid GetRequestingUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("List")]
        public async Task<IActionResult> GetAll([FromBody] AssignmentFilter filter)
        {
            if (User.IsInRole("Employee"))
                filter.UserId = GetRequestingUserId();

            var assignments = await _assignmentService.GetAllAsync(filter);
            return Ok(assignments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var assignment = await _assignmentService.GetByIdAsync(id);

            if (User.IsInRole("Employee") && assignment.UserId != GetRequestingUserId())
                return Forbid();

            return Ok(assignment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
        {
            // Validate that the admin in the DTO matches the requesting admin
            if (dto.ProcessedByAdminId != GetRequestingUserId())
                return Forbid();

            var assignment = await _assignmentService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = assignment.AssignmentId }, assignment);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssignmentDto dto)
        {
            await _assignmentService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var assignment = await _assignmentService.GetByIdAsync(id);
            if (assignment.UserId != GetRequestingUserId())
                return Forbid();

            await _assignmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
