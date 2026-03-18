using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IT_Asset_Management_System.Data;
using IT_Asset_Management_System.Repository.Interfaces;

namespace IT_Asset_Management_System.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly dbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(dbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.FromResult(true);
        }

        public virtual Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
