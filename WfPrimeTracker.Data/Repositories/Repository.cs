using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class Repository<T> : IRepository<T> where T : class, IPersistentItem {
        private readonly PrimeContext _context;

        public Repository(PrimeContext context) {
            _context = context;
        }

        protected virtual Expression<Func<T, T, T>> Updater { get; }

        protected virtual int PropertyUpdateCount { get; }

        /// <inheritdoc />
        public virtual async Task<T> Get(int id, Includes<T> includes = null) {
            var result = await GetBaseQuery(includes).FirstOrDefaultAsync(o => o.Id == id);
            return result;
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAll(Includes<T> includes = null) {
            var result = await GetBaseQuery(includes).ToArrayAsync();
            return result;
        }

        /// <inheritdoc />
        public virtual async Task<int> AddOrUpdate(T obj) {
            var result = await _context.Set<T>()
                                       .Upsert(obj)
                                       .On(o => o.Id)
                                       .WhenMatched(Updater)
                                       .RunAsync();
            _context.Database.CloseConnection();

            return result;
        }

        /// <inheritdoc />
        public virtual async Task<int> AddOrUpdateMany(IEnumerable<T> objs) {
            var result = await AddOrUpdateBatch(objs);
            return result;
        }

        /// <inheritdoc />
        public virtual Task<int> SaveChangesAsync() {
            return _context.SaveChangesAsync();
        }

        protected IQueryable<T> GetBaseQuery(Includes<T> includes) {
            return includes?.Invoke(_context.Set<T>()) ?? _context.Set<T>();
        }

        private async Task<int> AddOrUpdateBatch(IEnumerable<T> objs) {
            var result = 0;

            var propCount = 0;
            var toBatch = new List<T>();
            foreach (var obj in objs) {
                toBatch.Add(obj);
                propCount += PropertyUpdateCount + 1; //+1 since ID is also inserted

                if (propCount + PropertyUpdateCount + 1 >= 2000) {
                    result += await AddOrUpdateAll(toBatch);
                    propCount = 0;
                    toBatch.Clear();
                }
            }
            if (toBatch.Any()) {
                result += await AddOrUpdateAll(toBatch);
            }

            return result;
        }

        private async Task<int> AddOrUpdateAll(IEnumerable<T> objs) {
            var result = await _context.Set<T>()
                                       .UpsertRange(objs)
                                       .On(o => o.Id)
                                       .WhenMatched(Updater)
                                       .RunAsync();
            return result;
        }


    }
}