using System;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.Asset
{
    public class AssetDto
    {
        public Guid AssetId { get; set; }
        public required string AssetTag { get; set; } 
        public Guid ProductId { get; set; }
        public required string ProductName { get; set; } 
        public Guid CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public AssetStatus Status { get; set; }
    }
}
