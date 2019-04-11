using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    internal interface IIdProvider {
        int GetPersistentKey<T>(T obj) where T : IPersistentItem;
    }
}