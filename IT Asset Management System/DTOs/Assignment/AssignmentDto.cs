using System;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.Assignment
{
    public class AssignmentDto
    {
        public Guid AssignmentId { get; set; }
        public Guid RequestId { get; set; }
        public AssignmentStatus Status { get; set; }
        public Guid AssetId { get; set; }
        public required string AssetTag { get; set; }
        public required string ProductName { get; set; } 
        public required string CategoryName { get; set; } 
        public Guid UserId { get; set; }
        public required string Username { get; set; } 
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
