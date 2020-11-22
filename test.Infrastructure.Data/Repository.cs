using System;
using System.Collections.Generic;
using System.Text;
using test.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using test.Domain.Core;
using System.Threading.Tasks;

namespace test.Infrastructure.Data
{
    public class Repository<T, Y> : IRepository<T, Y> where T : class
    {
        private readonly BlogContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(BlogContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task Create(T t)
        {
            _context.Add(t);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T t)
        {
            _context.Update(t);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(T t)
        {
            _dbSet.Remove(t);
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<List<T>> FindAll()
        {
            return await _dbSet.ToListAsync();
        }

    }
}
