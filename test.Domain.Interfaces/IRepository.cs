using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Domain.Core;

namespace test.Domain.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task Create(Post post);
        Task Update(Post post);
        bool Any(int id);
        Task Remove(Post post);
        Task<Post> FirstOrDefaultAsync(int? id);
    }
}
