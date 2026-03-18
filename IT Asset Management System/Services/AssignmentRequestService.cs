using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.DTOs.AssignmentRequest;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Services.Interfaces;

namespace IT_Asset_Management_System.Services
{
    public class AssignmentRequestService : IAssignmentRequestService
    {
        private readonly IAssignmentRequestRepository _assignmentRequestRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignmentRequestService(IAssignmentRequestRepository assignmentRequestRepository, ICategoryRepository categoryRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _assignmentRequestRepository = assignmentRequestRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AssignmentRequestDto> AddAsync(CreateAssignmentRequestDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");

            var existing = await _assignmentRequestRepository.GetPendingRequestByCategoryAndUserAsync(dto.UserId, dto.CategoryId);
            if (existing != null)
                throw new ConflictException("You already have a pending request for this category.");

            var request = dto.ToEntity();

            await _assignmentRequestRepository.AddAsync(request);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");

            var Added = await _assignmentRequestRepository.GetByIdWithDetailsAsync(request.Id);

            if ( Added == null)
                throw new InternalServerException("Failed to retrieve the created assignment request.");

            return Added;
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

            await _assignmentRequestRepository.UpdateAsync(request);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
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

            await _assignmentRequestRepository.UpdateAsync(request);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var request = await _assignmentRequestRepository.GetByIdAsync(id);
            if (request == null)
                throw new NotFoundException("Assignment request not found.");

            if (request.Status == RequestStatus.Approved)
                throw new ValidationException("Approved assignment requests can not be deleted.");

            await _assignmentRequestRepository.DeleteAsync(request);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }
    }
}
