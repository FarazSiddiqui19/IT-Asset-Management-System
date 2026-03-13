using System;
using System.Collections.Generic;
using IT_Asset_Management_System.Entities.Enums; // Assuming enums will be here

namespace IT_Asset_Management_System.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required  string Email { get; set; } 
        public required string Username { get; set; } 
        public required string PasswordHash { get; set; } 
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   

        // Navigation properties
        public virtual List<AssignmentRequest> AssignmentRequests { get; set; } = new List<AssignmentRequest>();

      
        public virtual List<Ticket> CreatedTickets { get; set; } = new List<Ticket>();
        public virtual List<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}