using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Ticket;

namespace IT_Asset_Management_System.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto> AddAsync(CreateTicketDto dto);
        Task<TicketDto> GetByIdAsync(Guid id);
        Task<PagedResult<TicketDto>> GetAllAsync(TicketFilter filter);
        Task UpdateStatusAsync(Guid id, UpdateTicketStatusDto dto);
        Task UpdateContentAsync(Guid id, UpdateTicketContentDto dto);
        Task DeleteAsync(Guid Id , Guid UserId);
        
    }
}
