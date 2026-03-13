using System;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.AssignmentRequest
{
    public class AssignmentRequestDto
    {
        public Guid AssignmentRequestId { get; set; }
        public Guid UserId { get; set; }
        public required string Username { get; set; } 
        public Guid CategoryId { get; set; }
        public required string CategoryName { get; set; } 
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ProcessedByAdminId { get; set; }
        public string? ProcessedByAdminUsername { get; set; }
        public required string Description { get; set; }
    }
}
