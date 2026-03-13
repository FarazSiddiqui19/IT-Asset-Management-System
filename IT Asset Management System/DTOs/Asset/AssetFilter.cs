using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Enums;
using System;

namespace IT_Asset_Management_System.DTOs.Asset
{
    public class AssetFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public AssetStatus? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ProductId { get; set; }
        public AssetSortBy? SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }
}
