using System.Collections.Generic;
using System.Threading.Tasks;

namespace WfPrimeTracker.Data.Repositories {
    public interface IRepository<T> where T : class {
        Task<IEnumerable<T>> GetAll(Includes<T> includes = null);
        Task<int> AddOrUpdate(T obj);
        Task<int> AddOrUpdateMany(IEnumerable<T> objs);
        Task<int> SaveChangesAsync();
    }
}