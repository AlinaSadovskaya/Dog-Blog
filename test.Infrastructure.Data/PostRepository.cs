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
    public class PostRepository : Repository<Post, int>
    {
        private readonly BlogContext _context;
        public PostRepository(BlogContext context)
        : base(context)
        {
            _context = context;
        }

        public List<Post> FindAllByUser(string UserId)
        {
            return _context.Posts.Include(s => s.User).Where(s => s.UserId == UserId).ToList();
        }
        public List<Post> FindAllByTopic(int TopicId)
        {
            return _context.Posts.Include(s => s.Topic).Where(s => s.TopicId == TopicId).ToList();
        }
        public async Task<Post> FirstOrDefaultAsync(int? id)
        {
            return await _context.Posts.FirstOrDefaultAsync(m => m.PostId == id);
        }
      
    }
}
