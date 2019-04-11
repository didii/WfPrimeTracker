using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    public interface IRepository<T> where T : class, IPersistentItem {
        Task<T> Get(int id, Includes<T> includes = null);
        Task<IEnumerable<T>> GetAll(Includes<T> includes = null);
        Task<int> AddOrUpdate(T obj);
        Task<int> AddOrUpdateMany(IEnumerable<T> objs);
        Task<int> SaveChangesAsync();
    }
}
