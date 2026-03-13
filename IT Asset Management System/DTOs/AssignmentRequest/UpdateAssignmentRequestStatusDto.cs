using IT_Asset_Management_System.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.AssignmentRequest
{
    public class UpdateAssignmentRequestStatusDto
    {
        
        [Required]
        [AllowedValues(RequestStatus.Approved,RequestStatus.Rejected)]
        public RequestStatus Status { get; set; }

        [Required]
        public Guid ProcessedByAdminId { get; set; }
    }
}
