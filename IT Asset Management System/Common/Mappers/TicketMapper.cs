using IT_Asset_Management_System.DTOs.Ticket;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;
using System;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class TicketMapper
    {
        public static TicketDto ToDto(this Ticket ticket)
        {
            if (ticket == null) throw new ArgumentNullException(nameof(ticket));
            if (ticket.User == null) throw new InvalidOperationException("Ticket must be loaded with User before mapping");

            var assignedToUserId = ticket.AssignedToUser != null ? ticket.AssignedToUserId : null;

            string? assetTag = null;
            string? productName = null;

            if (ticket.Assignment != null && ticket.Assignment.Asset != null && ticket.Assignment.Asset.Product != null)
            {
                assetTag = ticket.Assignment.Asset.AssetTag;
                productName = ticket.Assignment.Asset.Product.Name;
            }

            return new TicketDto
            {
                TicketId = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt,
                UserId = ticket.UserId,
                Username = ticket.User.Username,
                AssignedToUserId = assignedToUserId,
                AssignedToUsername = ticket.AssignedToUser != null ? ticket.AssignedToUser.Username : null,
                AssetTag = assetTag,
                ProductName = productName
            };
        }


        public static Ticket ToEntity(this CreateTicketDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Ticket
            {
                UserId = dto.UserId,
                AssignmentId = dto.AssignmentId,
                Title = dto.Title,
                Description = dto.Description,
                Status = TicketStatus.Open
            };
        }
    }
}
