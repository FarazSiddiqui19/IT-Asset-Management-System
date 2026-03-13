using System.Linq;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Product;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Product p)
        {
            if(p == null) throw new InvalidOperationException("Product cannot be null when mapping to ProductDto.");

            if(p.Assets == null) throw new InvalidOperationException("Product's Assets collection cannot be null when mapping to ProductDto.");

            if(p.Category == null) throw new InvalidOperationException("Product's Category cannot be null when mapping to ProductDto.");


            return new ProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty,
                TotalAssets = p.Assets?.Count ?? 0,
                AvailableCount = p.Assets?.Count(a => a.Status == AssetStatus.Available ) ?? 0,
                AssignedCount = p.Assets?.Count(a => a.Status == AssetStatus.Assigned) ?? 0,
                UnderMaintenanceCount = p.Assets?.Count(a => a.Status == AssetStatus.UnderMaintenance) ?? 0,
                RetiredCount = p.Assets?.Count(a => a.Status == AssetStatus.Retired) ?? 0
            };
        }

        public static Product ToEntity(this CreateProductDto dto)
        {
            if(dto == null) throw new InvalidOperationException("ProductCreateDto cannot be null when mapping to Product.");
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };
        }
        
    }
}
