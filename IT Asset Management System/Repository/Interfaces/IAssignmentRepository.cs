using System;
using System.Threading.Tasks;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Assignment;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<PagedResult<AssignmentDto>> GetAllAsync(AssignmentFilter filter);
        Task<Assignment?> GetActiveAssignmentByAssetIdAsync(Guid assetId);
        Task<AssignmentDto?> GetByIdWithDetailsAsync(Guid id);
    }
}
