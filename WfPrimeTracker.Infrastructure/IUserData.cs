using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WfPrimeTracker.Infrastructure {
    public interface IUserData {
        /// <summary>
        /// If the user did not authenticate
        /// </summary>
        bool IsAnonymous { get; }

        /// <summary>
        /// If anonymous, this is the users ID
        /// </summary>
        Guid AnonymousId { get; }

        /// <summary>
        /// If not anonymous, this will be his 'identifier'
        /// </summary>
        string Email { get; }

        /// <summary>
        /// The requests IP address
        /// </summary>
        IPAddress Id { get; }
    }
}
