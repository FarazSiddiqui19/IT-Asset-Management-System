using System.Threading.Tasks;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Asset;
using System;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<PagedResult<AssetDto>> GetAllAsync(AssetFilter filter);
        Task<Asset?> GetByAssetTagAsync(string assetTag);
        Task<bool> HasAssetsAsync(Guid productId);
        Task<AssetDto?> GetByIdWithDetailsAsync(Guid id);
        Task<bool> HasAssignmentsAsync(Guid assetId);
    }
}
