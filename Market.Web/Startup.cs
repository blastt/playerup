using Hangfire;
using Market.Web.Hangfire;
using Microsoft.Owin;
using Owin;
using System.Globalization;

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
                .UseSqlServerStorage("MarketEntities");


            //JobHelper.SetSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });


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
