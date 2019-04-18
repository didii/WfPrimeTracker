using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Services {
    internal class PrimeItemService : IPrimeItemService {
        private readonly IPersistentRepository<PrimeItem> _repo;
        private readonly IMapper _mapper;

        public PrimeItemService(IPersistentRepository<PrimeItem> repo,
                                IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PrimeItemDto>> GetAll() {
            var entity = await _repo.GetAll(query => query
                                                    .Include(item => item.PrimePartIngredients)
                                                    .ThenInclude(i => i.PrimePart)
                                                    .Include(part => part.PrimePartIngredients)
                                                    .ThenInclude(i => i.RelicDrops)
                                                    .ThenInclude(drop => drop.Relic)
                                                    .Include(item => item.IngredientsGroups)
                                                    .ThenInclude(g => g.ResourceIngredients)
                                                    .ThenInclude(i => i.Resource));
            var result = _mapper.Map<IEnumerable<PrimeItemDto>>(entity);
            return result;
        }

        /// <inheritdoc />
        public async Task<PrimeItemDto> Get(int id) {
            var entity = await _repo.Get(id);
            var result = _mapper.Map<PrimeItemDto>(entity);
            return result;
        }

        /// <inheritdoc />
        public async Task<MemoryStream> GetImage(int id) {
            var entity = await _repo.Get(id, query => query.Include(i => i.Image));
            var result = new MemoryStream(entity.Image.Data);
            return result;
        }
    }
}