using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.AssignmentRequest;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Services
{
    public class AssignmentRequestService : IAssignmentRequestService
    {
        private readonly IAssignmentRequestRepository _assignmentRequestRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public AssignmentRequestService(IAssignmentRequestRepository assignmentRequestRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
        {
            _assignmentRequestRepository = assignmentRequestRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<AssignmentRequestDto> AddAsync(CreateAssignmentRequestDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            var existing = await _assignmentRequestRepository.GetPendingRequestByCategoryAndUserAsync(dto.UserId, dto.CategoryId);
            if (existing != null)
                throw new ConflictException("You already have a pending request for this category.");

            var request = new AssignmentRequest
            {
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Status = RequestStatus.Pending
            };

            await _assignmentRequestRepository.AddAsync(request);

            var full = await _assignmentRequestRepository.GetByIdWithDetailsAsync(request.Id);
            return full!;
        }

        public async Task<AssignmentRequestDto> GetByIdAsync(Guid id)
        {
            var dto = await _assignmentRequestRepository.GetByIdWithDetailsAsync(id);
            if (dto == null)
                throw new NotFoundException("Assignment request not found.");
            return dto;
        }

        public async Task<PagedResult<AssignmentRequestDto>> GetAllAsync(AssignmentRequestFilter filter)
        {
            return await _assignmentRequestRepository.GetAllAsync(filter);
        }

        public async Task UpdateContentAsync(Guid id, UpdateAssignmentRequestContentDto dto)
        {
            

            var request = await _assignmentRequestRepository.GetByIdAsync(id);
            if (request == null)
                throw new NotFoundException("Assignment request not found.");

            if (request.UserId != dto.UserId)
                throw new ForbiddenException("You can only update your own assignment requests.");

            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Only pending requests can be updated.");

            if (dto.CategoryId.HasValue)
            {
                var existing = await _assignmentRequestRepository.GetPendingRequestByCategoryAndUserAsync(dto.UserId, dto.CategoryId.Value);
                if (existing != null && existing.Id != id)
                    throw new ConflictException("You already have a pending request for this category.");

                var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null) throw new NotFoundException("Category not found.");

                request.CategoryId = dto.CategoryId.Value;
            }

            if (!string.IsNullOrEmpty(dto.Description))
                request.Description = dto.Description;

            var ok = await _assignmentRequestRepository.UpdateAsync(request);
            if (!ok) throw new ValidationException("Failed to update assignment request.");
        }

        public async Task UpdateStatusAsync(Guid id, UpdateAssignmentRequestStatusDto dto)
        {
            

            var request = await _assignmentRequestRepository.GetByIdAsync(id);
            if (request == null)
                throw new NotFoundException("Assignment request not found.");

            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Only pending requests can be rejected.");

            if (dto.Status != RequestStatus.Rejected)
                throw new ValidationException("Admins can only reject pending requests.");

            var admin = await _userRepository.GetByIdAsync(dto.ProcessedByAdminId);
            if (admin == null) throw new NotFoundException("Processing admin not found.");
            if (admin.Role != UserRole.Admin) throw new ValidationException("ProcessedByAdminId must belong to an admin user.");

            request.Status = RequestStatus.Rejected;
            request.ProcessedByAdminId = dto.ProcessedByAdminId;

            var ok = await _assignmentRequestRepository.UpdateAsync(request);
            if (!ok) throw new ValidationException("Failed to update assignment request.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var request = await _assignmentRequestRepository.GetByIdAsync(id);
            if (request == null)
                throw new NotFoundException("Assignment request not found.");

            if (request.Status == RequestStatus.Approved)
                throw new ValidationException("Approved assignment requests can not be deleted.");

            if (await _assignmentRequestRepository.HasActiveAssignmentAsync(id))
                throw new ValidationException("Assignment request cannot be deleted because an active assignment exists for it.");

            var ok = await _assignmentRequestRepository.DeleteAsync(request);
            if (!ok) throw new ValidationException("Failed to delete assignment request.");
        }
    }
}
