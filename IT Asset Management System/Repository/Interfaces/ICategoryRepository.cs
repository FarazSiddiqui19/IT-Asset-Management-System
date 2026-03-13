using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
        Task<List<CategoryDto>> GetAllAsync();
        Task<bool> HasRequestsAsync(Guid categoryId);
        Task<bool> HasActiveAssignmentsAsync(Guid categoryId);
    }
}
