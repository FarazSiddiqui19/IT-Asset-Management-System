using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Assignment
{
    public class CreateAssignmentDto
    {
        [Required]
        public Guid RequestId { get; set; }

        [Required]
        public Guid AssetId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProcessedByAdminId { get; set; }
    }
}
