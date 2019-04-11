using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Services {
    internal class PrimeItemService : IPrimeItemService {
        private readonly IIdProvider _idProvider;
        private readonly IRepository<PrimeItem> _repo;
        private readonly IRepository<Image> _imageRepo;
        private readonly IMapper _mapper;

        public PrimeItemService(IIdProvider idProvider, IRepository<PrimeItem> repo, IRepository<Image> imageRepo, IMapper mapper) {
            _idProvider = idProvider;
            _repo = repo;
            _imageRepo = imageRepo;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PrimeItemDto>> GetAll() {
            var entity = await _repo.GetAll(query => query
                                                    .Include(item => item.PrimeParts)
                                                        .ThenInclude(part => part.RelicDrops)
                                                        .ThenInclude(drop => drop.Relic)
                                                    .Include(item => item.Ingredients));
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
            var entity = await _imageRepo.Get(id);
            var result = new MemoryStream(entity.Data);
            return result;
        }

        private void AddPersistentKeys(PrimeItem item) {
            item.Id = _idProvider.GetPersistentKey(item);
            foreach (var primePart in item.PrimeParts) {
                primePart.Id = _idProvider.GetPersistentKey(primePart);
                foreach (var relicDrop in primePart.RelicDrops) {
                    relicDrop.Id = _idProvider.GetPersistentKey(relicDrop);
                    relicDrop.Relic.Id = _idProvider.GetPersistentKey(relicDrop.Relic);
                }
            }
        }
    }
}