using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    class ElementFinder : IElementFinder {
        /// <inheritdoc />
        public HtmlNode GetTableOfSimpleRewardsTable(HtmlDocument document) {
            var result = document.DocumentNode.SelectSingleNode(@"//table");
            return result;
        }

        /// <inheritdoc />
        public IEnumerable<HtmlNode> GetRowOfSimpleRewardsTable(HtmlNode node) {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (node.Name != "table") throw new ArgumentException("Argument must a table node", nameof(node));

            foreach (var row in node.ChildNodes) {
                if (row.Name != "tr")
                    continue;
                yield return row;
            }
        }
    }
}