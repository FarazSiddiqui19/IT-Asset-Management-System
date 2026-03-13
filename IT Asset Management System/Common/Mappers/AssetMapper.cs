using System;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Asset;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class AssetMapper
    {
        public static AssetDto ToDto(this Asset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (asset.Product == null) throw new InvalidOperationException("Asset must be loaded with Product and Category before mapping");
            if (asset.Product.Category == null) throw new InvalidOperationException("Asset.Product must be loaded with Category before mapping");

            return new AssetDto
            {
               
                AssetId = asset.Id,
                AssetTag = asset.AssetTag,
                ProductId = asset.ProductId,
                ProductName = asset.Product.Name,
                CategoryId = asset.Product.CategoryId,
                CategoryName = asset.Product.Category.Name,
                PurchaseDate = asset.PurchaseDate,
                Status = asset.Status
            };
        }
    }
}
