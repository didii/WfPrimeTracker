using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using WfPrimeTracker.Infrastructure;
using WfPrimeTracker.Server.Models;

namespace WfPrimeTracker.Server.Middleware {
    public class AuthMiddleware {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IUserData userData) {
            var ip = httpContext.Connection.RemoteIpAddress;
            if (httpContext.Request.Headers.TryGetValue("AnonymousId", out var anonIdValues) && anonIdValues.Count >= 1 && Guid.TryParse(anonIdValues[0], out var anonId)) {
                ((UserData)userData).InitializeAnonymous(ip, anonId);
            }

            return _next(httpContext);
        }
    }
}
