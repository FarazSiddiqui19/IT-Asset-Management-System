using System;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.Ticket
{
    public class TicketDto
    {
        public Guid TicketId { get; set; }
        public required string Title { get; set; } 
        public required string Description { get; set; } 
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public required string Username { get; set; } 
        public Guid? AssignedToUserId { get; set; }
        public string? AssignedToUsername { get; set; }
        public string? AssetTag { get; set; }
        public string? ProductName { get; set; }
    }
}
