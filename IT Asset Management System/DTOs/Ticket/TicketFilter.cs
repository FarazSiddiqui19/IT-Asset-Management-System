using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Enums;
using System;

namespace IT_Asset_Management_System.DTOs.Ticket
{
    public class TicketFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public TicketStatus? Status { get; set; }
        public Guid? UserId { get; set; }
        public Guid? OpenOrAssignedToUserId { get; set; }
        public Guid? AssignedTo { get; set; }
        public TicketSortBy? SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }
}
