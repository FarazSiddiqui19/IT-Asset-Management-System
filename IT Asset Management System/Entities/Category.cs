using System.Collections.Generic;

namespace IT_Asset_Management_System.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual List<Product> Products { get; set; } = new List<Product>();
        public virtual List<AssignmentRequest> AssignmentRequests { get; set; } = new List<AssignmentRequest>();
    }
}