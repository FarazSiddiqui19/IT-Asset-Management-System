using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Product
{
    public class UpdateProductDto
    {
       
        [MaxLength(100)]
        public string? Name { get; set; }

        
        [MaxLength(500)]
        public string? Description { get; set; }

        
        public Guid? CategoryId { get; set; }
    }
}
