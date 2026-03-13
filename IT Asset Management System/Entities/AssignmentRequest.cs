using System;
using System.Collections.Generic;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Entities
{
    public class AssignmentRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
       
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public Guid? ProcessedByAdminId { get; set; }

        // Navigation property for the admin who processed the request
        public virtual User? ProcessedByAdmin { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!; // Required navigation property
        public virtual Category Category { get; set; } = null!; // Required navigation property
        public virtual Assignment? Assignment { get; set; } // Changed from ICollection to single Assignment
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}