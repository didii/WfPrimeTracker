using System.Collections.Generic;
using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    public interface IElementFinder {
        HtmlNode GetTableOfSimpleRewardsTable(HtmlDocument document);
        IEnumerable<HtmlNode> GetRowOfSimpleRewardsTable(HtmlNode node);
    }
}