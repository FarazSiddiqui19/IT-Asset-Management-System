using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Assignment;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface IAssignmentService
    {
        Task<AssignmentDto> AddAsync(CreateAssignmentDto dto);
        Task<AssignmentDto> GetByIdAsync(Guid id);
        Task<PagedResult<AssignmentDto>> GetAllAsync(AssignmentFilter filter);
        Task UpdateAsync(Guid id, UpdateAssignmentDto dto);
        Task DeleteAsync(Guid id);
    }
}
