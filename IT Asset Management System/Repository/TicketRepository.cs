using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Ticket;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Enums;
using IT_Asset_Management_System.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(dbContext context) : base(context)
        {
        }

        public async Task<PagedResult<TicketDto>> GetAllAsync(TicketFilter filter)
        {
            IQueryable<Ticket> query = _dbSet
                .Include(t => t.User)
                .Include(t => t.AssignedToUser)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.Asset)
                        .ThenInclude(asst => asst.Product);

            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status.Value);

            if (filter.UserId.HasValue)
                query = query.Where(t => t.UserId == filter.UserId.Value);

            if (filter.AssignedTo.HasValue)
                query = query.Where(t => t.AssignedToUserId == filter.AssignedTo.Value);

            if (filter.OpenOrAssignedToUserId.HasValue)
                query = query.Where(t =>
                    (t.Status == TicketStatus.Open && t.AssignedToUserId == null) ||
                    t.AssignedToUserId == filter.OpenOrAssignedToUserId.Value);

            bool asc = filter.SortOrder == SortOrder.Ascending;

            switch (filter.SortBy)
            {
                case TicketSortBy.CreatedAt:
                    query = filter.SortOrder == SortOrder.Ascending
                        ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt);
                    break;
                default:
                    query = filter.SortOrder == SortOrder.Ascending
                        ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt);
                    break;
            }

            var total = await query.CountAsync();
            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(t => t.ToDto())
                .ToListAsync();

            return new PagedResult<TicketDto> { Items = items, TotalCount = total };
        }

        public async Task<TicketDto?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.AssignedToUser)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.Asset)
                        .ThenInclude(asst => asst.Product)
                .Where(t => t.Id == id)
                .Select(t => t.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasCommentsAsync(Guid ticketId)
        {
            return await _context.Comments.AnyAsync(c => c.TicketId == ticketId);
        }
    }
}
