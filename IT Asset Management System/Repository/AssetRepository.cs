using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.DTOs.Asset;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Enums;
using IT_Asset_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset_Management_System.Repository
{
    public class AssetRepository : GenericRepository<Asset>, IAssetRepository
    {
        public AssetRepository(dbContext context) : base(context)
        {
        }

        public async Task<PagedResult<AssetDto>> GetAllAsync(AssetFilter filter)
        {
            IQueryable<Asset> query = _dbSet.Include(a => a.Product).ThenInclude(p => p.Category);

            if (filter.Status.HasValue)
                query = query.Where(a => a.Status == filter.Status.Value);

            if (filter.ProductId.HasValue)
                query = query.Where(a => a.ProductId == filter.ProductId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(a => a.Product.CategoryId == filter.CategoryId.Value);

            bool asc = filter.SortOrder == SortOrder.Ascending;
            switch (filter.SortBy)
            {
                case AssetSortBy.PurchaseDate:
                    query = asc ? query.OrderBy(a => a.PurchaseDate) : 
                                  query.OrderByDescending(a => a.PurchaseDate);
                    break;
                default:
                    query = asc ? query.OrderBy(a => a.AssetTag) : 
                                  query.OrderByDescending(a => a.AssetTag);
                    break;
            }

            var total = await query.CountAsync();
            var items = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).Select(a => a.ToDto()).ToListAsync();
          

            return new PagedResult<AssetDto> { Items = items, TotalCount = total };
        }

        public async Task<Asset?> GetByAssetTagAsync(string assetTag)
        {
           
            return await _dbSet.FirstOrDefaultAsync(a => a.AssetTag == assetTag);
        }
        
        public async Task<bool> HasAssetsAsync(Guid productId)
        {
            return await _dbSet.AnyAsync(a => a.ProductId == productId);
        }

        public async Task<AssetDto?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.Product)
                    .ThenInclude(p => p.Category)
                .Where(a => a.Id == id)
                .Select(a => a.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasAssignmentsAsync(Guid assetId)
        {
          
            return await _context.Assignments.AnyAsync(a => a.AssetId == assetId );
        }
    }
}
