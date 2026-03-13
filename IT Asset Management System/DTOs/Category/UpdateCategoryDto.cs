using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}
