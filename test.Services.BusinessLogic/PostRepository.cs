using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Domain.Core;
using test.Domain.Interfaces;
using test.Infrastructure.Data;

namespace test.Services.BusinessLogic
{
    public class PostRepository : IRepository
    {
        private readonly BlogContext _context;
        public PostRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task Create(Post post)
        {
            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Post post)
        {
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.Posts.Any(e => e.PostId == id); ;
        }
        public async Task Remove(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> FirstOrDefaultAsync(int? id)
        {
            return await _context.Posts.FirstOrDefaultAsync(m => m.PostId == id);
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
