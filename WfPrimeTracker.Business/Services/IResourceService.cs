using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Services {
    public interface IResourceService {
        Task<Stream> GetImage(int id);
    }

    class ResourceService : IResourceService {
        private readonly IPersistentRepository<Resource> _repo;

        public ResourceService(IPersistentRepository<Resource> repo) {
            _repo = repo;
        }

        /// <inheritdoc />
        public async Task<Stream> GetImage(int id) {
            var resource = await _repo.Get(id, query => query.Include(r => r.Image));
            var result = new MemoryStream(resource.Image.Data);
            return result;
        }
    }
}
