using System;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    class ElementParser : IElementParser {
        private readonly IElementFinder _finder;

        public ElementParser(IElementFinder finder) {
            _finder = finder;
        }

        /// <inheritdoc />
        public RowData ParseRowOfSimpleRewardsTable(HtmlNode row) {
            if (row == null) throw new ArgumentNullException(nameof(row));
            if (row.Name != "tr") throw new ArgumentException("Argument must be a table row HtmlNode", nameof(row));

            var col = 0;
            var rowData = new RowData();
            foreach (var cell in row.ChildNodes) {
                if (cell.Name != "td")
                    continue;

                switch (col) {
                    case 0:
                        ParseItemCell(cell, rowData);
                        break;
                    case 1:
                        ParsePartCell(cell, rowData);
                        break;
                    case 2:
                        ParseRelicTierCell(cell, rowData);
                        break;
                    case 3:
                        ParseRelicNameCell(cell, rowData);
                        break;
                    case 4:
                        ParseRelicRarityCell(cell, rowData);
                        break;
                    case 5:
                        ParseVaulted(cell, rowData);
                        break;
                }
                col++;
            }

            if (rowData.ItemName == null || rowData.ItemName == "Forma")
                return null;
            if (!rowData.DropChance.HasValue)
                return null;
            if (!rowData.IsVaulted.HasValue)
                return null;
            if (!rowData.RelicTier.HasValue)
                return null;
            return rowData;
        }

        private void ParseItemCell(HtmlNode cell, RowData data) {
            var a = cell.ChildNodes.First(c => c.Name == "a");
            var href = a.Attributes.First(attr => attr.Name == "href");
            data.ItemName = HttpUtility.HtmlDecode(a.InnerText);
            data.ItemUrl = href.Value;
        }

        private void ParsePartCell(HtmlNode cell, RowData data) {
            data.PartName = HttpUtility.HtmlDecode(cell.InnerText);
        }

        private void ParseRelicTierCell(HtmlNode cell, RowData data) {
            var text = cell.InnerText;
            if (Enum.TryParse<RelicTier>(text, true, out var tier))
                data.RelicTier = tier;
        }

        private void ParseRelicNameCell(HtmlNode cell, RowData data) {
            var a = cell.SelectSingleNode(".//a");
            var href = a.Attributes.First(attr => attr.Name == "href");
            data.RelicName = HttpUtility.HtmlDecode(a.InnerText);
            data.RelicUrl = href.Value;
        }

        private void ParseRelicRarityCell(HtmlNode cell, RowData data) {
            var text = cell.InnerText;
            if (Enum.TryParse<DropChance>(text, true, out var rarity)) {
                data.DropChance = rarity;
            }
        }

        private void ParseVaulted(HtmlNode cell, RowData data) {
            var text = cell.InnerText.Trim();
            if (text.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                data.IsVaulted = true;
            else if (text.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                data.IsVaulted = false;
        }
    }
}