using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Asset;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Entities.Enums;
using System.Linq.Expressions;

namespace IT_Asset_Management_System.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IProductRepository _productRepository;

        public AssetService(IAssetRepository assetRepository, IProductRepository productRepository)
        {
            _assetRepository = assetRepository;
            _productRepository = productRepository;
        }

        public async Task<AssetDto> AddAsync(CreateAssetDto dto)
        {
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                throw new NotFoundException("Product not found.");

            var existing = await _assetRepository.GetByAssetTagAsync(dto.AssetTag);
            if (existing != null)
                throw new ConflictException("Asset with this tag already exists.");

            var asset = new Asset
            {
                AssetTag = dto.AssetTag,
                PurchaseDate = dto.PurchaseDate,
                ProductId = dto.ProductId,
                Status = AssetStatus.Available
            };

            await _assetRepository.AddAsync(asset);

            var full = await _assetRepository.GetByIdWithDetailsAsync(asset.Id);
            return full!;
        }

        public async Task<AssetDto> GetByIdAsync(Guid id)
        {
            var dto = await _assetRepository.GetByIdWithDetailsAsync(id);
            if (dto == null)
                throw new NotFoundException("Asset not found.");
            return dto;
        }

        public async Task<PagedResult<AssetDto>> GetAllAsync(AssetFilter filter)
        {
            return await _assetRepository.GetAllAsync(filter);
        }

        private Dictionary<AssetStatus, List<AssetStatus>> allowedTransitions = new Dictionary<AssetStatus, List<AssetStatus>>
        {
            { AssetStatus.Available, new List<AssetStatus> {  AssetStatus.UnderMaintenance, AssetStatus.Retired } },
            { AssetStatus.UnderMaintenance, new List<AssetStatus> { AssetStatus.Available, AssetStatus.Retired } }
        };

        public async Task UpdateAsync(Guid id, UpdateAssetDto dto)
        {
            var asset = await _assetRepository.GetByIdAsync(id);

            if (asset == null)
                throw new NotFoundException("Asset not found.");

            if(!allowedTransitions.ContainsKey(asset.Status) || !allowedTransitions[asset.Status].Contains(dto.Status))
                throw new ValidationException($"Invalid status transition from {asset.Status} to {dto.Status}.");
       

            var ok = await _assetRepository.UpdateAsync(asset);
            if (!ok)
                throw new ValidationException("Failed to update asset.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null)
                throw new NotFoundException("Asset not found.");

            if (asset.Status != AssetStatus.Available)
                throw new ValidationException("Only available assets can be deleted.");

            var hasAssignments = await _assetRepository.HasAssignmentsAsync(id);
            if (hasAssignments)
                throw new ValidationException("Asset cannot be deleted because assignment history exists.");

            var ok = await _assetRepository.DeleteAsync(asset);
            if (!ok)
                throw new ValidationException("Failed to delete asset.");
        }
    }
}
