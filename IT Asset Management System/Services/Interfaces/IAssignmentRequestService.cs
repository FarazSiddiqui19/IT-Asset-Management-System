using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.AssignmentRequest;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface IAssignmentRequestService
    {
        Task<AssignmentRequestDto> AddAsync(CreateAssignmentRequestDto dto);
        Task<AssignmentRequestDto> GetByIdAsync(Guid id);
        Task<PagedResult<AssignmentRequestDto>> GetAllAsync(AssignmentRequestFilter filter);
        Task UpdateContentAsync(Guid id, UpdateAssignmentRequestContentDto dto);
        Task UpdateStatusAsync(Guid id, UpdateAssignmentRequestStatusDto dto);
     
        Task DeleteAsync(Guid id);
    }
}
