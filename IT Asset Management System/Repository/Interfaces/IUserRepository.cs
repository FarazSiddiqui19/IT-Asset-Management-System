using System.Threading.Tasks;
using IT_Asset_Management_System.Entities;

namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> HasRequestsAsync(Guid userId);
        Task<bool> HasTicketsAsync(Guid userId);
        Task<bool> HasCommentsAsync(Guid userId);
        Task<bool> HasActiveAssignmentsAsync(Guid userId);
    }
}
