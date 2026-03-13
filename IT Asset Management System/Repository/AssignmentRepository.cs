using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.DTOs.Assignment;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Enums;
using IT_Asset_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset_Management_System.Repository
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(dbContext context) : base(context)
        {
        }

        public async Task<PagedResult<AssignmentDto>> GetAllAsync(AssignmentFilter filter)
        {
            IQueryable<Assignment> query = _dbSet
                .Include(a => a.Asset)
                    .ThenInclude(asst => asst.Product)
                        .ThenInclude(p => p.Category)
                .Include(a => a.Request)
                    .ThenInclude(r => r.User);

            if (filter.Status.HasValue)
                query = query.Where(a => a.Status == filter.Status.Value);

            if (filter.AssetId.HasValue)
                query = query.Where(a => a.AssetId == filter.AssetId.Value);

            if (filter.UserId.HasValue)
                query = query.Where(a => a.Request.UserId == filter.UserId.Value);

            bool asc = filter.SortOrder == SortOrder.Ascending;
            switch (filter.SortBy)
            {
                case AssignmentSortBy.ReturnDate:
                    query = asc ? query.OrderBy(a => a.ReturnDate) : query.OrderByDescending(a => a.ReturnDate);
                    break;
                default:
                    query = asc ? query.OrderBy(a => a.AssignedDate) : query.OrderByDescending(a => a.AssignedDate);
                    break;
            }

            var total = await query.CountAsync();
            var items = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Select(a=>a.ToDto()).Take(filter.PageSize).ToListAsync();
      

            return new PagedResult<AssignmentDto> { Items = items, TotalCount = total };
        }

        public async Task<Assignment?> GetActiveAssignmentByAssetIdAsync(Guid assetId)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AssetId == assetId && a.Status == AssignmentStatus.Active);
        }

        public async Task<AssignmentDto?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(a => a.Asset)
                    .ThenInclude(asst => asst.Product)
                        .ThenInclude(p => p.Category)
                .Include(a => a.Request)
                    .ThenInclude(r => r.User)
                .Where(a => a.Id == id)
                .Select(a => a.ToDto())
                .FirstOrDefaultAsync();
        }
    }
}
