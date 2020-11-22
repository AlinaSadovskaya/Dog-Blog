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
    public class DogRepository : Repository<Dog, int>
    {
        private readonly BlogContext _context;
        public DogRepository(BlogContext context)
        : base(context)
        {
            _context = context;
        }

        public bool Any(int id)
        {
            return _context.Dogs.Any(e => e.DogId == id); ;
        }

        public async Task<Dog> FirstOrDefaultAsync(int? id)
        {
            return await _context.Dogs.FirstOrDefaultAsync(m => m.DogId == id);
        }
    }
}
