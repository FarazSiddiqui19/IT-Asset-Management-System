using System;
using System.Threading.Tasks;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.AssignmentRequest;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface IAssignmentRequestRepository : IRepository<AssignmentRequest>
    {
        Task<PagedResult<AssignmentRequestDto>> GetAllAsync(AssignmentRequestFilter filter);
        Task<AssignmentRequest?> GetPendingRequestByCategoryAndUserAsync(Guid userId, Guid categoryId);
        Task<AssignmentRequestDto?> GetByIdWithDetailsAsync(Guid id);
        Task<bool> HasActiveAssignmentAsync(Guid requestId);
        Task<bool> HasCommentsAsync(Guid requestId);
    }
}
