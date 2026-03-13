using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Common;
using IT_Asset_Management_System.DTOs.Ticket;
using System.Threading.Tasks;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<PagedResult<TicketDto>> GetAllAsync(TicketFilter filter);
        Task<TicketDto?> GetByIdWithDetailsAsync(Guid id);
        Task<bool> HasCommentsAsync(Guid ticketId);
    }
}
