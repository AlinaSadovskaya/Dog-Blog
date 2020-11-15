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
    public class CommentRepository : IRepository<Comment, int>
    {
        private readonly BlogContext _context;
        public CommentRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task Create(Comment comment)
        {
            _context.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Comment comment)
        {
            _context.Update(comment);
            await _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id); ;
        }
        public async Task Remove(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> FirstOrDefaultAsync(int? id)
        {
            return await _context.Comments.FirstOrDefaultAsync(m => m.CommentId == id);
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
