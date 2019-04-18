using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    public interface IElementParser {
        RowData ParseRowOfSimpleRewardsTable(HtmlNode row);
    }
}