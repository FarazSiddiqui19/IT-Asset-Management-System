using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [Required]
        public required Guid CategoryId { get; set; }
    }
}
