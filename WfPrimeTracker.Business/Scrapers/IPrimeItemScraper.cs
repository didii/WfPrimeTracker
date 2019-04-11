using System.Threading.Tasks;

namespace WfPrimeTracker.Business.Scrapers {
    internal interface IPrimeItemScraper {
        Task<PrimeItemData> GetData(string wikiUrl);
    }
}