using System;

namespace IT_Asset_Management_System.DTOs.Product
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; } 
        public required string Description { get; set; } 
        public Guid CategoryId { get; set; }
        public required string CategoryName { get; set; } 
        public int TotalAssets { get; set; }
        public int AvailableCount { get; set; }
        public int AssignedCount { get; set; }
        public int UnderMaintenanceCount { get; set; }
        public int RetiredCount { get; set; }
    }
}
