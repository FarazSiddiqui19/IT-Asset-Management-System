using IT_Asset_Management_System.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Comment
{
    public class CreateCommentDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public CommentType CommentType { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? AssignmentRequestId { get; set; }

        [Required]
        [MaxLength(2000)]
        public required string Content { get; set; }
    }
}
