using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.DTOs.AssignmentRequest;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
   
    public class AssignmentRequestController : ControllerBase
    {
        private readonly IAssignmentRequestService _assignmentRequestService;

        public AssignmentRequestController(IAssignmentRequestService assignmentRequestService)
        {
            _assignmentRequestService = assignmentRequestService;
        }

        private Guid GetRequestingUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);



        [HttpPost("List")]

        public async Task<IActionResult> GetAll([FromBody] AssignmentRequestFilter filter)
        {
            if (User.IsInRole("Employee"))
                filter.UserId = GetRequestingUserId();

            var requests = await _assignmentRequestService.GetAllAsync(filter);
            return Ok(requests);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(Guid id)
        {
            var request = await _assignmentRequestService.GetByIdAsync(id);

            return Ok(request);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentRequestDto dto)
        {
            if (dto.UserId != GetRequestingUserId())
                return Forbid();

            var request = await _assignmentRequestService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = request.AssignmentRequestId }, request);
        }

        [HttpPatch]
        [Route("UpdateContent/{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateContent(Guid id, [FromBody] UpdateAssignmentRequestContentDto dto)
        {
            

            if (dto.UserId != GetRequestingUserId())
                return Forbid();

            await _assignmentRequestService.UpdateContentAsync(id, dto);
            return Ok();
        }

        [HttpPatch]
        [Route("UpdateStatus/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateAssignmentRequestStatusDto dto)
        {
            

            if (dto.ProcessedByAdminId != GetRequestingUserId())
                return Forbid();

            await _assignmentRequestService.UpdateStatusAsync(id, dto);
            return Ok();
        }

    

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = await _assignmentRequestService.GetByIdAsync(id);
            if (request.UserId != GetRequestingUserId())
                return Forbid();

            await _assignmentRequestService.DeleteAsync(id);
            return NoContent();
        }
    }
}
