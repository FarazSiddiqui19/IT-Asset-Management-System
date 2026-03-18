using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Product;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Enums;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(dbContext context) : base(context)
        {
        }

        public async Task<PagedResult<ProductDto>> GetAllAsync(ProductFilter filter)
        {
            IQueryable<Product> query = _dbSet.Include(p => p.Category).Include(p => p.Assets);
                

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(p => p.Name.Contains(filter.Name));

            bool asc = filter.SortOrder == SortOrder.Ascending;
            switch (filter.SortBy)
            {
                case ProductSortBy.Name:
                    query = asc ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                    break;
                default:
                    query = asc ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);
                    break;
            }

            var total = await query.CountAsync();
            var items = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).Select(p => p.ToDto()).ToListAsync();



            return new PagedResult<ProductDto> { Items = items, TotalCount = total };
        }

        public async Task<ProductDto?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Assets)
                .Where(p => p.Id == id)
                .Select(p => p.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasProductsAsync(Guid categoryId)
        {
            return await _dbSet.AnyAsync(p => p.CategoryId == categoryId);
        }
    }
}
