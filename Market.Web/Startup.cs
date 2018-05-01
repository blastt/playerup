using Market.Web.ModelBinders;
using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Market.Web.Startup))]
namespace Market.Web
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru");
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
