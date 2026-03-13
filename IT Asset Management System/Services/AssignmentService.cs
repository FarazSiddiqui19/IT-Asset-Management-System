using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Assignment;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentRequestRepository _assignmentRequestRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUserRepository _userRepository;

        public AssignmentService(
            IAssignmentRepository assignmentRepository,
            IAssignmentRequestRepository assignmentRequestRepository,
            IAssetRepository assetRepository,
            IUserRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentRequestRepository = assignmentRequestRepository;
            _assetRepository = assetRepository;
            _userRepository = userRepository;
        }

        public async Task<AssignmentDto> AddAsync(CreateAssignmentDto dto)
        {
            // Validate user
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");

            // Validate asset
            var asset = await _assetRepository.GetByIdAsync(dto.AssetId);
            if (asset == null)
                throw new NotFoundException("Asset not found.");

            if (asset.Status != AssetStatus.Available)
                throw new ValidationException("Asset is not available for assignment.");

            // Validate request
            var request = await _assignmentRequestRepository.GetByIdAsync(dto.RequestId);
            if (request == null)
                throw new NotFoundException("Assignment request not found.");

            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Assignment request is not pending.");

            if (request.UserId != dto.UserId)
                throw new ValidationException("Assignment request does not belong to the specified user.");

            // Check active assignment
            var active = await _assignmentRepository.GetActiveAssignmentByAssetIdAsync(dto.AssetId);
            if (active != null)
                throw new ValidationException("Asset already has an active assignment.");

            var assignment = new Assignment
            {
                RequestId = dto.RequestId,
                AssetId = dto.AssetId,
                Status = AssignmentStatus.Active,
                AssignedDate = DateTime.UtcNow
            };

            await _assignmentRepository.AddAsync(assignment);

            // Update asset status to Assigned
            asset.Status = AssetStatus.Assigned;
            await _assetRepository.UpdateAsync(asset);

            // Update request status to Approved and record which admin processed it
            request.Status = RequestStatus.Approved;
            request.ProcessedByAdminId = dto.ProcessedByAdminId;
            await _assignmentRequestRepository.UpdateAsync(request);

            var full = await _assignmentRepository.GetByIdWithDetailsAsync(assignment.Id);
            return full!;
        }

        public async Task<AssignmentDto> GetByIdAsync(Guid id)
        {
            var dto = await _assignmentRepository.GetByIdWithDetailsAsync(id);
            if (dto == null)
                throw new NotFoundException("Assignment not found.");
            return dto;
        }

        public async Task<PagedResult<AssignmentDto>> GetAllAsync(AssignmentFilter filter)
        {
            return await _assignmentRepository.GetAllAsync(filter);
        }

        public async Task UpdateAsync(Guid id, UpdateAssignmentDto dto)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null)
                throw new NotFoundException("Assignment not found.");

            if (assignment.Status != AssignmentStatus.Active)
                throw new ValidationException("Only active assignments can be returned.");

            if (dto.Status != AssignmentStatus.Returned)
                throw new ValidationException("Assignments can only be set to Returned.");

            var asset = await _assetRepository.GetByIdAsync(assignment.AssetId);
            if (asset == null)
                throw new NotFoundException("Asset not found.");

            assignment.Status = AssignmentStatus.Returned;
            assignment.ReturnDate = DateTime.UtcNow;

            // Update asset status to Available
            asset.Status = AssetStatus.Available;
            await _assetRepository.UpdateAsync(asset);

            var ok = await _assignmentRepository.UpdateAsync(assignment);
            if (!ok)
                throw new ValidationException("Failed to update assignment.");
        }

        public async Task DeleteAsync(Guid id)
        {
            // Allow deletion only when the assignment has been returned
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) throw new NotFoundException("Assignment not found.");

            if (assignment.Status != AssignmentStatus.Returned)
                throw new ValidationException("Only returned assignments can be deleted.");

            var ok = await _assignmentRepository.DeleteAsync(assignment);
            if (!ok) throw new ValidationException("Failed to delete assignment.");
            
        }

      
    }
}
