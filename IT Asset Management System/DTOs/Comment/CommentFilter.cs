using IT_Asset_Management_System.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Comment
{
    public class CommentFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        [Required]
        public CommentType Type { get; set; }
        public Guid? TicketId { get; set; }
        public Guid? AssignmentRequestId { get; set; }

        
    }
}
