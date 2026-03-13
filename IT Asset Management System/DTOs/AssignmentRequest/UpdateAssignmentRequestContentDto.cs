using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.AssignmentRequest
{
    public class UpdateAssignmentRequestContentDto
    {
     

        [Required]
        public Guid UserId { get; set; }

        public Guid? CategoryId { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
