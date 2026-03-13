using IT_Asset_Management_System.DTOs.Category;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddAsync(CreateCategoryDto dto);
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task<List<CategoryDto>> GetAllAsync();
        Task UpdateAsync(Guid id, UpdateCategoryDto dto);
        Task DeleteAsync(Guid id);
    }
}
