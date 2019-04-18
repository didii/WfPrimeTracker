using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;

namespace WfPrimeTracker.Business.Tests {
    public class VoidRelicByRewardsScraperTests {
        private VoidRelicByRewardsScraper _sut;

        private IPrimeItemScraper _primeItemScraper = new PrimeItemScraper();
        private IIdProvider _idProvider = new IdProvider();
        private PrimeContext _context;

        [SetUp]
        public void SetUp() {
            _primeItemScraper = new PrimeItemScraper();
            _idProvider = new IdProvider();
            var contextOptions = new DbContextOptionsBuilder<PrimeContext>()
                                .UseInMemoryDatabase("PrimeContextTest")
                                .Options;
            _context = new PrimeContext(contextOptions);

            _sut = new VoidRelicByRewardsScraper(_primeItemScraper, _idProvider, _context);
        }

        [Test]
        public async Task TestScrape() {
            await _sut.FetchAllPrimeItemData();
        }
    }
}
