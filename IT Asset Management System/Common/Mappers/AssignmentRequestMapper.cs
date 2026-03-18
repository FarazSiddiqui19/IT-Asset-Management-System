using System;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.AssignmentRequest;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class AssignmentRequestMapper
    {
        public static AssignmentRequestDto ToDto(this AssignmentRequest assignmentRequest)
        {
            if (assignmentRequest == null) throw new ArgumentNullException(nameof(assignmentRequest));
            if (assignmentRequest.User == null) throw new InvalidOperationException("AssignmentRequest must be loaded with User before mapping");
            if (assignmentRequest.Category == null) throw new InvalidOperationException("AssignmentRequest must be loaded with Category before mapping");

            return new AssignmentRequestDto
            {
                AssignmentRequestId = assignmentRequest.Id,
                UserId = assignmentRequest.UserId,
                Username = assignmentRequest.User.Username,
                CategoryId = assignmentRequest.CategoryId,
                CategoryName = assignmentRequest.Category.Name,
                Status = assignmentRequest.Status,
                CreatedAt = assignmentRequest.CreatedAt,
                ProcessedByAdminId = assignmentRequest.ProcessedByAdminId,
                ProcessedByAdminUsername = assignmentRequest.ProcessedByAdmin != null ? assignmentRequest.ProcessedByAdmin.Username : null,
                Description = assignmentRequest.Description
            };
        }

        public static AssignmentRequest ToEntity(this CreateAssignmentRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new AssignmentRequest
            {
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                Description = dto.Description,
                Status = Entities.Enums.RequestStatus.Pending
            };
        }
    }
}
