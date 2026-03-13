using System;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.DTOs.Comment;

namespace IT_Asset_Management_System.Common.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(this Comment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            if (comment.User == null) throw new InvalidOperationException("Comment must be loaded with User before mapping.");

            if(comment.AssignmentRequestId == null && comment.TicketId == null)
                throw new InvalidOperationException("Comment must be associated with either a Ticket or an AssignmentRequest.");

            return new CommentDto
            {
                CommentId = comment.Id,
                TicketId = comment.TicketId,
                AssignmentRequestId = comment.AssignmentRequestId,
                Type = comment.Type,
                UserId = comment.UserId,
                Username = comment.User.Username,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
