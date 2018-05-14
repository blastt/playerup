using Autofac;
using Autofac.Integration.Mvc;
using Hangfire;
using Hangfire.Common;
using Market.Model.Models;
using Market.Service;
using Market.Web.Hangfire;
using Market.Web.ModelBinders;
using Marketplace.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Market.Web.Startup))]
namespace Market.Web
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            MarketHangfire.ConfigureHangfire(app);
            GlobalConfiguration.Configuration
			    .UseSqlServerStorage(@"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=PalyerUpDbOrig;Integrated Security=True");

            JobHelper.SetSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });


            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });
            app.UseHangfireServer();

           
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //});
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru");
            
            app.MapSignalR();
        }
    }
}
