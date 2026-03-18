using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.DTOs.Category;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset_Management_System.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(dbContext context) : base(context)
        {
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(c => c.Name)
                .Select(c => c.ToDto())
                .ToListAsync();
        }

        public async Task<bool> HasRequestsAsync(Guid categoryId)
        {
            return await _context.AssignmentRequests.AnyAsync(ar => ar.CategoryId == categoryId);
        }

        public async Task<bool> HasAnyAssignmentsAsync(Guid categoryId)
        {
           
            return await _context.Assignments
                .Include(a => a.Request)
                .AnyAsync(a => a.Request.CategoryId == categoryId);
        }
    }
}
