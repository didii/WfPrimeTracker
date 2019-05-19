using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WfPrimeTracker.Infrastructure;

namespace WfPrimeTracker.Server.Models {
    public class UserData : IUserData, IDisposable {
        private bool _isAnonymous;
        private Guid _anomymousId;
        private string _email;
        private IPAddress _ip;

        public bool IsInitialized { get; private set; } = false;

        public bool IsAnonymous => Ensure(_isAnonymous);

        public Guid AnonymousId => Ensure(_anomymousId);

        public string Email => Ensure(_email);

        public IPAddress Id => Ensure(_ip);

        public void InitializeAnonymous(IPAddress ip, Guid anonymousId) {
            _isAnonymous = true;
            _anomymousId = anonymousId;
            _email = null;
            IsInitialized = true;
        }

        public void InitializeOauth(IPAddress ip, string email, Guid? guid = null) {
            _isAnonymous = false;
            _anomymousId = guid.HasValue ? guid.Value : Guid.Empty;
            _email = email;
            IsInitialized = true;
        }

        public void Dispose() {
            IsInitialized = false;
        }

        private T Ensure<T>(T value) {
            if (!IsInitialized)
                throw new UserDataNotInitializedException();
            return value;
        }
    }

    public class UserDataNotInitializedException : Exception {
        public UserDataNotInitializedException() : base("UserData was not initialized before use") {}
    }
}
