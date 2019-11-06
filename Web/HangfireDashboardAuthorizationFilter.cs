using Hangfire.Dashboard;
using System;

namespace Web
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            return httpcontext.User?.Identity?.IsAuthenticated??false;
        }
    }
}
