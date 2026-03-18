using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Common.Mappers;
using IT_Asset_Management_System.DTOs.Assignment;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.Services.Interfaces;

namespace IT_Asset_Management_System.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignmentRequestRepository _assignmentRequestRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignmentService(
            IAssignmentRepository assignmentRepository,
            IAssignmentRequestRepository assignmentRequestRepository,
            IAssetRepository assetRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _assignmentRepository = assignmentRepository;
            _assignmentRequestRepository = assignmentRequestRepository;
            _assetRepository = assetRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AssignmentDto> AddAsync(CreateAssignmentDto dto)
        {
            // Validate user
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null) throw new NotFoundException("User not found.");

            // Validate asset
            var asset = await _assetRepository.GetByIdAsync(dto.AssetId);
            if (asset == null) throw new NotFoundException("Asset not found.");
            if (asset.Status != AssetStatus.Available)
                throw new ValidationException("Asset is not available for assignment.");

            // Validate request
            var request = await _assignmentRequestRepository.GetByIdAsync(dto.RequestId);
            if (request == null) throw new NotFoundException("Assignment request not found.");
            if (request.Status != RequestStatus.Pending)
                throw new ValidationException("Assignment request is not pending.");
            if (request.UserId != dto.UserId)
                throw new ValidationException("Assignment request does not belong to the specified user.");

            // Validate category match
            var product = await _productRepository.GetByIdAsync(asset.ProductId);
            if (product == null) throw new NotFoundException("Product not found.");
            if (product.CategoryId != request.CategoryId)
                throw new ValidationException("Asset category does not match the requested category.");

            // Create assignment
            var assignment = dto.ToEntity();
            await _assignmentRepository.AddAsync(assignment);

            // Update asset status
            asset.Status = AssetStatus.Assigned;
            await _assetRepository.UpdateAsync(asset);

            // Update request status
            request.Status = RequestStatus.Approved;
            request.ProcessedByAdminId = dto.ProcessedByAdminId;
            await _assignmentRequestRepository.UpdateAsync(request);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");

            var Added = await _assignmentRepository.GetByIdWithDetailsAsync(assignment.Id);

            if(Added == null)
                throw new InternalServerException("Failed to retrieve the created assignment. Please try again.");

            return Added;
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

            await _assignmentRepository.UpdateAsync(assignment);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }

        public async Task DeleteAsync(Guid id)
        {
            // Allow deletion only when the assignment has been returned
            var assignment = await _assignmentRepository.GetByIdAsync(id);
            if (assignment == null) throw new NotFoundException("Assignment not found.");

            if (assignment.Status != AssignmentStatus.Returned)
                throw new ValidationException("Only returned assignments can be deleted.");

            await _assignmentRepository.DeleteAsync(assignment);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }

      
    }
}
