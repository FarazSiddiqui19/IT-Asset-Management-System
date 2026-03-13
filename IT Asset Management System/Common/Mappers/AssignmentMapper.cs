using System;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Assignment;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class AssignmentMapper
    {
        public static AssignmentDto ToDto(this Assignment assignment)
        {
            if (assignment == null) throw new ArgumentNullException(nameof(assignment));
            if (assignment.Asset == null) throw new InvalidOperationException("Assignment must be loaded with Asset before mapping");
            if (assignment.Asset.Product == null) throw new InvalidOperationException("Assignment.Asset must be loaded with Product before mapping");
            if (assignment.Asset.Product.Category == null) throw new InvalidOperationException("Assignment.Asset.Product must be loaded with Category before mapping");

            // Assignment.Request is expected to be present and carry User info
            if (assignment.Request == null || assignment.Request.User == null)
                throw new InvalidOperationException("Assignment must be loaded with User before mapping");

            return new AssignmentDto
            {
                AssignmentId = assignment.Id,
                RequestId = assignment.RequestId,
                Status = assignment.Status,
                AssetId = assignment.AssetId,
                AssetTag = assignment.Asset.AssetTag,
                ProductName = assignment.Asset.Product.Name,
                CategoryName = assignment.Asset.Product.Category.Name,
                UserId = assignment.Request.UserId,
                Username = assignment.Request.User.Username,
                AssignedDate = assignment.AssignedDate,
                ReturnDate = assignment.ReturnDate
            };
        }
    }
}
