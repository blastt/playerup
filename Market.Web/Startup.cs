using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Market.Web.Startup))]
namespace Market.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
