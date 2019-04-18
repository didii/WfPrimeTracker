using System.Threading.Tasks;
using Hangfire.Server;

namespace WfPrimeTracker.Business.Jobs {
    public interface IHangfireJob {
        Task Invoke(PerformContext context);
    }
}