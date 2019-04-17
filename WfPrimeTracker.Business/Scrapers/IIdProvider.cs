using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    internal interface IIdProvider {
        T InsertPersistentKey<T>(T obj) where T : IPersistentItem;
    }
}