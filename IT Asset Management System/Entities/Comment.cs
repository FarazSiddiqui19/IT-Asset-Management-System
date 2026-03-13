using IT_Asset_Management_System.Entities.Enums;
using System;

namespace IT_Asset_Management_System.Entities
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? AssignmentRequestId { get; set; }
        public CommentType Type { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Ticket? Ticket { get; set; }
        public virtual AssignmentRequest? AssignmentRequest { get; set; }
    }
}