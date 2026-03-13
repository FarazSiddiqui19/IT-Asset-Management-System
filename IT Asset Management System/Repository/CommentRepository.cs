using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Comment;
using IT_Asset_Management_System.Common.Mappers;

namespace IT_Asset_Management_System.Repository
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(dbContext context) : base(context)
        {
        }
        public async Task<CommentDto?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.User)
                .Where(c => c.Id == id)
                .Select(c => c.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task<List<CommentDto>> GetAllAsync(CommentFilter filter)
        {
            IQueryable<Comment> query = _dbSet.Include(c => c.User);

            if (filter.TicketId.HasValue)
                query = query.Where(c => c.TicketId == filter.TicketId.Value);

            if (filter.AssignmentRequestId.HasValue)
                query = query.Where(c => c.AssignmentRequestId == filter.AssignmentRequestId.Value);

            return await query
                .OrderBy(c => c.CreatedAt)
                .Select(c => c.ToDto())
                .ToListAsync();
        }
    }
}
