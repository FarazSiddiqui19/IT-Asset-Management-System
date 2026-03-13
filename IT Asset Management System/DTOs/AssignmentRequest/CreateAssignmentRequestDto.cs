using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.AssignmentRequest
{
    public class CreateAssignmentRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }
    }
}
