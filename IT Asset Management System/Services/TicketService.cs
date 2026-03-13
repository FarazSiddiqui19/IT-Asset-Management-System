using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.Repository.Interfaces;
using IT_Asset_Management_System.DTOs.Ticket;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.Common.Exceptions;
using IT_Asset_Management_System.Entities.Enums;
using IT_Asset_Management_System.Common.Mappers;

namespace IT_Asset_Management_System.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IAssignmentRepository _assignmentRepository;
     

        public TicketService(ITicketRepository ticketRepository, IAssignmentRepository assignmentRepository)
        {
            _ticketRepository = ticketRepository;
            _assignmentRepository = assignmentRepository;
  
        }

        public async Task<TicketDto> AddAsync(CreateTicketDto dto)
        {
            if (dto.AssignmentId.HasValue)
            {
                var assignmentDto = await _assignmentRepository.GetByIdWithDetailsAsync(dto.AssignmentId.Value);
                if (assignmentDto == null)
                    throw new NotFoundException("Assignment not found.");

                if (assignmentDto.UserId != dto.UserId)
                    throw new ValidationException("You can only raise a ticket for your own assignment.");
            }

            var ticket = dto.ToEntity();

            await _ticketRepository.AddAsync(ticket);

            var full = await _ticketRepository.GetByIdWithDetailsAsync(ticket.Id);
            return full!;
        }

        public async Task<TicketDto> GetByIdAsync(Guid id)
        {
            var dto = await _ticketRepository.GetByIdWithDetailsAsync(id);
            if (dto == null)
                throw new NotFoundException("Ticket not found.");
            return dto;
        }

        public async Task<PagedResult<TicketDto>> GetAllAsync(TicketFilter filter)
        {
            return await _ticketRepository.GetAllAsync(filter);
        }


        
        public async Task UpdateStatusAsync(Guid id, UpdateTicketStatusDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                throw new NotFoundException("Ticket not found.");

            if (ticket.Status == TicketStatus.Open)
            {
                if (dto.Status == TicketStatus.InProgress)
                {
                    ticket.Status = dto.Status;
                    ticket.AssignedToUserId = dto.AdminId;
                }


                else
                   throw new ValidationException("Open tickets can only be moved to In Progress.");

            }

            else if(ticket.Status == TicketStatus.InProgress)
            { 
                if(dto.Status == TicketStatus.Closed)
                {
                    ticket.Status = dto.Status;
                    ticket.AssignedToUserId = dto.AdminId;
                }


                else
                    throw new ValidationException("Open tickets can only be moved to In Progress.");

            }

            else
                throw new ValidationException("Only open or in progress tickets can be updated.");



            var ok = await _ticketRepository.UpdateAsync(ticket);
            if (!ok)
                throw new ValidationException("Failed to update ticket.");
        }


        public async Task UpdateContentAsync(Guid id, UpdateTicketContentDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) throw new NotFoundException("Ticket not found.");

            if (ticket.UserId != dto.UserId)
                throw new ForbiddenException("You can only update your own tickets.");

            if (ticket.Status != TicketStatus.Open)
                throw new ValidationException("Only open tickets can be updated.");

            if (dto.Title != null)
                ticket.Title = dto.Title;

            if (dto.Description != null)
                ticket.Description = dto.Description;

            var ok = await _ticketRepository.UpdateAsync(ticket);
            if (!ok)
                throw new ValidationException("Failed to update ticket.");
        }

        public async Task DeleteAsync(Guid Id , Guid UserId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(Id);
            if (ticket == null) throw new NotFoundException("Ticket not found.");

            if (ticket.UserId != UserId)
                throw new InvalidOperationException("You can delete your own ticket");

            if (ticket.Status == TicketStatus.InProgress)
                throw new ValidationException("In progress tickets cannot be deleted");

            var ok = await _ticketRepository.DeleteAsync(ticket);
            if (!ok) throw new ValidationException("Failed to delete ticket.");
        }




    }
}
