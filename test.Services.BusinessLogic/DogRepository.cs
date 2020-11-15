using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using test.Domain.Core;
using test.Domain.Interfaces;
using test.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace test.Services.BusinessLogic
{
    public class DogRepository : IRepository<Dog, int>
    {
        private readonly BlogContext _context;
        public DogRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task Create(Dog dog)
        {
            _context.Add(dog);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Dog dog)
        {
            _context.Update(dog);
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.Dogs.Any(e => e.DogId == id); ;
        }
        public async Task Remove(Dog dog)
        {
            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
        }

        public async Task<Dog> FirstOrDefaultAsync(int? id)
        {
            return await _context.Dogs.FirstOrDefaultAsync(m => m.DogId == id);
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

    }
}
