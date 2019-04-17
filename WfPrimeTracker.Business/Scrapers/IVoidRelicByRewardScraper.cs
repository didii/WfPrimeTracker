using System.Collections.Generic;
using System.Threading.Tasks;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Scrapers {
    internal interface IVoidRelicByRewardScraper {
        Task FetchAllPrimeItemData();
    }
}