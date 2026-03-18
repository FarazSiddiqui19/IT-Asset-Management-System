using System.Collections.Generic;

namespace IT_Asset_Management_System.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Description { get; set; } 
        public Guid CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        // Navigation properties
        public virtual Category Category { get; set; } = null!; // Required navigation property
        public virtual List<Asset> Assets { get; set; } = new List<Asset>(); 
    }
}