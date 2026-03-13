using System;
using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.AssignmentRequest
{
    public class UpdateAssignmentRequestDto
    {
        public Guid? CategoryId { get; set; }
        public string? Description { get; set; }
        public RequestStatus? Status { get; set; }
        public Guid ProcessedByAdminId { get; set; }
    }
}
