using System;
using System.Collections.Generic;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Entities
{
    public class Assignment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RequestId { get; set; } // Foreign key to AssignmentRequest
        public Guid AssetId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; } // Nullable as per business rule "ReturnDate must be set when returned."
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Active;
       

        // Navigation properties
        public virtual AssignmentRequest Request { get; set; } = null!; // Required navigation property
        public virtual Asset Asset { get; set; } = null!; // Required navigation property
        public virtual List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}