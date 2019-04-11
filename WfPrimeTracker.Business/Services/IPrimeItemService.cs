using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Services {
    public interface IPrimeItemService {
        Task<IEnumerable<PrimeItemDto>> GetAll();
        Task<PrimeItemDto> Get(int id);
        Task<MemoryStream> GetImage(int id);
    }
}
