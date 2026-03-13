using IT_Asset_Management_System.Entities.Enums;
using System;

namespace IT_Asset_Management_System.DTOs.Comment
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public Guid? TicketId { get; set; }
        public Guid UserId { get; set; }
        public Guid? AssignmentRequestId { get; set; }
        public CommentType Type { get; set; }
        public required string Username { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
