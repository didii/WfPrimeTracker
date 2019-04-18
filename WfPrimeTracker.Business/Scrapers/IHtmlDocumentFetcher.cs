using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    public interface IHtmlDocumentFetcher {
        Task<HtmlDocument> GetPage(string url);
    }
}