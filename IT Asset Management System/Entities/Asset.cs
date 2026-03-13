using System;
using System.Collections.Generic;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Entities
{
    public class Asset
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string AssetTag { get; set; }
        public required DateTime PurchaseDate { get; set; }
        public Guid ProductId { get; set; }
        public AssetStatus Status { get; set; } = AssetStatus.Available;


        // Navigation properties
        public virtual Product Product { get; set; } = null!; // Required navigation property
        public virtual List<Assignment> Assignments { get; set; } = new List<Assignment>();
    }
}