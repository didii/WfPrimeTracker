using System;
using System.Collections.Generic;
using System.Text;
using WfPrimeTracker.Domain.Users;

namespace WfPrimeTracker.Domain.Contracts {
    public interface ITrackable {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset ModifiedOn { get; set; }
    }
}
