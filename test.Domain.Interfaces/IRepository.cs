using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Domain.Core;

namespace test.Domain.Interfaces
{
    public interface IRepository<T, Y> : IDisposable
        where T : class
    {
        Task Create(T post);
        Task Update(T post);
       // bool Any(Y id);
        Task Remove(T post);
      //  Task<T> FirstOrDefaultAsync(int? id);
    }
}
