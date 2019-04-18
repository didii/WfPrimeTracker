using System.Threading.Tasks;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    public interface IDataPersister {
        Task<PrimeItem> AddOrUpdateRowData(RowData rowData);
        Task<PrimeItem> AddOrUpdatePrimeItem(PrimeItem primeItem, PrimeItemData itemData);
        Task<Image> AddOrUpdateImage(Image image);
    }
}