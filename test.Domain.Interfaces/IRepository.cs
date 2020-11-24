using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Domain.Core;

namespace test.Domain.Interfaces
{
    public interface IRepository<T, Y> 
        where T : class
    {
        Task Create(T post);
        Task Update(T post);
        Task<List<T>> FindAll();
        Task Remove(T post);
        Task<T> FindAsyncById(Y id);
    }
}
