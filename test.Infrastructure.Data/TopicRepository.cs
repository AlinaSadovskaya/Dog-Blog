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
    public class TopicRepository : Repository<Topic, int>
    {
        private readonly BlogContext _context;
        public TopicRepository(BlogContext context)
        : base(context)
        {
            _context = context;
        }
        public bool Any(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id); 
        }
        public async Task<Topic> FirstOrDefaultAsync(int? id)
        {
            return await _context.Topics.FirstOrDefaultAsync(m => m.TopicId == id);
        }

    }
}
