using System.Linq;

namespace WfPrimeTracker.Data {
    public delegate IQueryable<T> Includes<T>(IQueryable<T> query);
}