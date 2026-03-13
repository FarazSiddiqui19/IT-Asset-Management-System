using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Asset;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface IAssetService
    {
        Task<AssetDto> AddAsync(CreateAssetDto dto);
        Task<AssetDto> GetByIdAsync(Guid id);
        Task<PagedResult<AssetDto>> GetAllAsync(AssetFilter filter);
        Task UpdateAsync(Guid id, UpdateAssetDto dto);
        Task DeleteAsync(Guid id);
    }
}
