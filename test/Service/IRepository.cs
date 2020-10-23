using test.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Service
{
    interface IRepository : IDisposable
    {
        Task Create(Post post);
        Task Update(Post post);
        bool Any(int id);
        Task Remove(Post post);
        Task<Post> FirstOrDefaultAsync(int? id);
    }
}
