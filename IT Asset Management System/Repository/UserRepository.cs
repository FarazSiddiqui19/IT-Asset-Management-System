using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset_Management_System.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(dbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            
            return await _dbSet.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<bool> HasRequestsAsync(Guid userId)
        {
            return await _context.AssignmentRequests.AnyAsync(ar => ar.UserId == userId);
        }

        public async Task<bool> HasTicketsAsync(Guid userId)
        {
            // Only consider open tickets as blocking deletion
            return await _context.Tickets.AnyAsync(t => t.UserId == userId && t.Status == TicketStatus.Open);
        }

        public async Task<bool> HasCommentsAsync(Guid userId)
        {
            return await _context.Comments.AnyAsync(c => c.UserId == userId);
        }

        public async Task<bool> HasActiveAssignmentsAsync(Guid userId)
        {
            return await _context.Assignments
                                    .AnyAsync(a => a.Status == AssignmentStatus.Active
                                                        && a.Request != null
                                                        && a.Request.UserId == userId);
        }
    }
    
}

