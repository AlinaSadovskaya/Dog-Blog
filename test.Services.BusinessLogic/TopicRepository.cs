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
    public class TopicRepository : IRepository<Topic, int>
    {
        private readonly BlogContext _context;
        public TopicRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task Create(Topic topic)
        {
            _context.Add(topic);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Topic topic)
        {
            _context.Update(topic);
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id); 
        }
        public async Task Remove(Topic topic)
        {
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
        }

        public async Task<Topic> FirstOrDefaultAsync(int? id)
        {
            return await _context.Topics.FirstOrDefaultAsync(m => m.TopicId == id);
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
        public async Task<List<Topic>> FindAll()
        {
            return await _context.Topics.ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
