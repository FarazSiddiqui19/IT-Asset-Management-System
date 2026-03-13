using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(2000)]
        public required string Content { get; set; }
    }
}
