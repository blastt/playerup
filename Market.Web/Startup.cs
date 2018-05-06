using Market.Model.Models;
using Market.Web.ModelBinders;
using Marketplace.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Globalization;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Market.Web.Startup))]
namespace Market.Web
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
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
