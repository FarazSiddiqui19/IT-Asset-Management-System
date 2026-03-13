using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.AssignmentRequest;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Enums;
using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Repository
{
    public class AssignmentRequestRepository : GenericRepository<AssignmentRequest>, IAssignmentRequestRepository
    {
        public AssignmentRequestRepository(dbContext context) : base(context)
        {
        }

        public async Task<PagedResult<AssignmentRequestDto>> GetAllAsync(AssignmentRequestFilter filter)
        {
            IQueryable<AssignmentRequest> query = _dbSet
                .Include(r => r.User)
                .Include(r => r.Category)
                .Include(r => r.ProcessedByAdmin);

            if (filter.Status.HasValue)
                query = query.Where(r => r.Status == filter.Status.Value);

            if (filter.UserId.HasValue)
                query = query.Where(r => r.UserId == filter.UserId.Value);

            if (filter.CategoryId.HasValue)
                query = query.Where(r => r.CategoryId == filter.CategoryId.Value);

            bool asc = filter.SortOrder == SortOrder.Ascending;
            switch (filter.SortBy)
            {
                case AssignmentRequestSortBy.Status:
                    query = asc ? query.OrderBy(r => r.Status) : query.OrderByDescending(r => r.Status);
                    break;
                default:
                    query = asc ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt);
                    break;
            }

            var total = await query.CountAsync();
            var items = await query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).Select(ar => ar.ToDto()).ToListAsync();
            

            return new PagedResult<AssignmentRequestDto> { Items = items, TotalCount = total };
        }

        public async Task<AssignmentRequest?> GetPendingRequestByCategoryAndUserAsync(Guid userId, Guid categoryId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.UserId == userId && r.CategoryId == categoryId && r.Status == RequestStatus.Pending);
        }

        public async Task<AssignmentRequestDto?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(ar => ar.User)
                .Include(ar => ar.Category)
                .Include(ar => ar.ProcessedByAdmin)
                .Where(ar => ar.Id == id)
                .Select(ar => ar.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasActiveAssignmentAsync(Guid requestId)
        {
            return await _context.Assignments.Where(a=>a.Status == AssignmentStatus.Active).AnyAsync(a => a.RequestId == requestId);
        }

        public async Task<bool> HasCommentsAsync(Guid requestId)
        {
            return await _context.Comments.AnyAsync(c => c.AssignmentRequestId == requestId);
        }
    }
}
