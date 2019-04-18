using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    class HtmlDocumentFetcher : IHtmlDocumentFetcher {
        /// <inheritdoc />
        public async Task<HtmlDocument> GetPage(string url) {
            var web = new HtmlWeb();
            var result = await web.LoadFromWebAsync(url);
            return result;
        }
    }
}