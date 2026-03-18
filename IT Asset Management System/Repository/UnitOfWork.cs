using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset_Management_System.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly dbContext _context;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(dbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex.InnerException?.Message);
                return false;
            }
        }
    }
}
