using System;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; } // Employee who created the ticket
        public Guid? AssignmentId { get; set; } // Nullable as per schema
        public required string Title { get; set; } 
        public required string Description { get; set; } 
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public Guid? AssignedToUserId { get; set; } // Admin who is assigned the ticket
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual User User { get; set; } = null!; // Employee who created the ticket
        public virtual Assignment? Assignment { get; set; } // Optional navigation property
        public virtual User? AssignedToUser { get; set; }  // Admin assigned to the ticket
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}