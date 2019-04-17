using System;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    public interface IPersistentRepository<T> : IRepository<T> where T : class, IPersistentItem {
        Task<T> Get(int id, Includes<T> includes = null);
    }
}
