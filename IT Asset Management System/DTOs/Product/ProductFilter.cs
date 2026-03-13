using System;
using IT_Asset_Management_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Product
{
    public class ProductFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public ProductSortBy? SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }
}
