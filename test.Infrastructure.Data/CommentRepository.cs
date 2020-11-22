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
    public class CommentRepository : Repository<Comment, int>
    {
        private readonly BlogContext _context;
        public CommentRepository(BlogContext context)
        : base(context)
        {
            _context = context;
        }

        public List<Comment> FindAllByPost(int PostId)
        {
            return _context.Comments.Include(s => s.Post).Where(s => s.PostId == PostId).ToList();
        }
        public List<Comment> FindAllByUser(User user)
        {
            return _context.Comments.Include(s => s.User).Where(s => s.User == user).ToList();
        }

    }
}
