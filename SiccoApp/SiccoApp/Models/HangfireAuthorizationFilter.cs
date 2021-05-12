using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiccoApp.Models
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        bool IDashboardAuthorizationFilter.Authorize([NotNull] DashboardContext context)
        {
            if ((HttpContext.Current.User.Identity != null) && (HttpContext.Current.User.IsInRole("AdminRole")))
                    return HttpContext.Current.User.Identity.IsAuthenticated;
            return false;
        }
    }
}