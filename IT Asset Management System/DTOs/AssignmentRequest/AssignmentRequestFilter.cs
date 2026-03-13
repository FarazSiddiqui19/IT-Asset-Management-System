using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Enums;
using System;

namespace IT_Asset_Management_System.DTOs.AssignmentRequest
{
    public class AssignmentRequestFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public RequestStatus? Status { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public AssignmentRequestSortBy? SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }
}
