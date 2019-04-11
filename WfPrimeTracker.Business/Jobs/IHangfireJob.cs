using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Business.Services;

namespace WfPrimeTracker.Business.Jobs {
    public interface IHangfireJob {
        Task Invoke();
    }
}
