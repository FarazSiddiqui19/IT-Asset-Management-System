using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.DTOs.Ticket;

namespace IT_Asset_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        private Guid GetRequestingUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("List")]
        public async Task<IActionResult> GetAll([FromBody] TicketFilter filter)
        {
            if (User.IsInRole("Employee"))
                filter.UserId = GetRequestingUserId();
            else
                filter.OpenOrAssignedToUserId = GetRequestingUserId();

            var tickets = await _ticketService.GetAllAsync(filter);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);

            if (User.IsInRole("Employee") && ticket.UserId != GetRequestingUserId())
                return Forbid();

            return Ok(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
        {
            if(dto.UserId != GetRequestingUserId())
                return Forbid();

            var ticket = await _ticketService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
        }

        [HttpPatch]
        [Route("UpdateStatus/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTicketStatusDto dto)
        {
            if (dto.AdminId != GetRequestingUserId())
                return Forbid();

            await _ticketService.UpdateStatusAsync(id, dto);
            return Ok();
        }


        [HttpPatch]
        [Route("UpdateContent/{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> UpdateContent(Guid id, [FromBody] UpdateTicketContentDto dto)
        {
            if(dto.UserId != GetRequestingUserId())
                return Forbid();

            await _ticketService.UpdateContentAsync(id, dto);
            return Ok();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(Guid id)
        {
            

            await _ticketService.DeleteAsync(id , GetRequestingUserId());
            return NoContent(); 
        }
       
    }
}
