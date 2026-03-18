using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Comment;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Mappers;

namespace IT_Asset_Management_System.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IAssignmentRequestRepository _assignmentRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository commentRepository, ITicketRepository ticketRepository, 
                                          IAssignmentRequestRepository assignmentRequestRepository, 
                                          IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _ticketRepository = ticketRepository;
            _assignmentRequestRepository = assignmentRequestRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CommentDto> AddAsync(CreateCommentDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId)
                if(user == null) throw new NotFoundException("User not found.");

            var isAdmin = user.Role == UserRole.Admin;
            Comment comment;

            if (dto.CommentType == CommentType.Ticket)
            {
                if (!dto.TicketId.HasValue)
                    throw new ValidationException("TicketId must be provided for ticket comments.");

                var ticket = await _ticketRepository.GetByIdAsync(dto.TicketId.Value)
                if(ticket == null) throw new NotFoundException("Ticket not found.");

                AuthorizeTicketComment(user, ticket, isAdmin);

                comment = dto.ToEntity();
            }
            else if (dto.CommentType == CommentType.AssignmentRequest)
            {
                if (!dto.AssignmentRequestId.HasValue)
                    throw new ValidationException("AssignmentRequestId must be provided for assignment request comments.");

                var request = await _assignmentRequestRepository.GetByIdAsync(dto.AssignmentRequestId.Value)
                if(request == null) throw new NotFoundException("Assignment request not found.");

                AuthorizeRequestComment(user, request, isAdmin);

                comment = dto.ToEntity();
            }
            else
            {
                throw new ValidationException("Invalid comment type.");
            }

            await _commentRepository.AddAsync(comment);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");

            var Added = await _commentRepository.GetByIdWithDetailsAsync(comment.Id) 
                
                if(Added == null)
                            throw new InternalServerException("Failed to retrieve the added comment. Please try again.");
           

            return Added;   
        }

        private void AuthorizeTicketComment(User user, Ticket ticket, bool isAdmin)
        {
            if (ticket.Status == TicketStatus.Closed)
                throw new ForbiddenException("You cannot comment on closed tickets.");

            if (ticket.Status == TicketStatus.Open)
            {
                if (isAdmin || ticket.UserId == user.Id)
                    return;

                throw new ForbiddenException("You do not have permission to comment on this ticket.");
            }

            if (ticket.Status == TicketStatus.InProgress)
            {
                if (ticket.AssignedToUserId == user.Id || ticket.UserId == user.Id)
                    return;

                throw new ForbiddenException("You do not have permission to comment on this ticket.");
            }
        }

        private void AuthorizeRequestComment(User user, AssignmentRequest request, bool isAdmin)
        {
           
            if (request.Status == RequestStatus.Rejected )
                throw new ForbiddenException("You cannot comment on rejected assignment requests.");

            if (request.Status == RequestStatus.Pending)
            {
                if (isAdmin || request.UserId == user.Id)
                    return;

                throw new ForbiddenException("You do not have permission to comment on this assignment request.");
            }

            if (request.Status == RequestStatus.Approved)
            {
                if (request.ProcessedByAdminId == user.Id || request.UserId == user.Id)
                    return;

                throw new ForbiddenException("You do not have permission to comment on this assignment request.");
            }
        }


        public async Task<PagedResult<CommentDto>> GetAllAsync(CommentFilter filter)
        {
            if (!filter.TicketId.HasValue && !filter.AssignmentRequestId.HasValue)
                throw new ValidationException("Either TicketId or AssignmentRequestId must be provided.");

            if (filter.TicketId.HasValue && filter.AssignmentRequestId.HasValue)
                throw new ValidationException("Cannot filter by both TicketId and AssignmentRequestId.");

            if (filter.TicketId.HasValue)
            {
                var ticket = await _ticketRepository.GetByIdAsync(filter.TicketId.Value);
                if (ticket == null) throw new NotFoundException("Ticket not found.");
            }
            else
            {
                var request = await _assignmentRequestRepository.GetByIdAsync(filter.AssignmentRequestId!.Value);
                if (request == null) throw new NotFoundException("Assignment request not found.");
            }

            return await _commentRepository.GetAllAsync(filter);
        }


        public async Task<CommentDto> GetByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdWithDetailsAsync(id);
            if (comment == null) throw new NotFoundException("Comment not found.");
            return comment;
        }

        public async Task UpdateAsync(Guid id, UpdateCommentDto dto)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) throw new NotFoundException("Comment not found.");

            if (comment.UserId != dto.UserId)
                throw new ForbiddenException("You can only update your own comments.");

            comment.Content = dto.Content;

            await _commentRepository.UpdateAsync(comment);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }

        public async Task DeleteAsync(Guid id, Guid requestingUserId)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null) throw new NotFoundException("Comment not found.");

            if (comment.UserId != requestingUserId)
                throw new ForbiddenException("You can only delete your own comments.");

            await _commentRepository.DeleteAsync(comment);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new InternalServerException("Failed to complete the operation. Please try again.");
        }
    }
}
