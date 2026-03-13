using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.DTOs.Comment;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentController(ICommentService commentService)
        {
            _service = commentService;
        }

        private Guid GetRequestingUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);



        [HttpPost("List")]
        public async Task<IActionResult> GetAll([FromBody] CommentFilter filter)
        {
            var comments = await _service.GetAllAsync(filter);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comment = await _service.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
        {
            if (dto.UserId != GetRequestingUserId())
                return Forbid();

            var comment = await _service.AddAsync(dto);

            return CreatedAtAction(nameof(GetAll), comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentDto dto)
        {
            if(dto.UserId != GetRequestingUserId())
                return Forbid();

            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id, GetRequestingUserId());
            return NoContent();
        }
    }
}
