﻿using Hangfire.Dashboard;
using Microsoft.Owin;

namespace Market.Web.Hangfire
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // In case you need an OWIN context, use the next line, `OwinContext` class
            // is the part of the `Microsoft.Owin` package.
            var owinContext = new OwinContext(context.GetOwinEnvironment());

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            bool result = owinContext.Authentication.User.IsInRole("Admin");
            return result;
        }
    }
}