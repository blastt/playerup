using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Market.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: null,
            //    url: "{prof}",
            //    defaults: new { controller = "Profile", action = "Info" }
            //);


            //routes.MapRoute(
            //   name: "ListRoute",
            //   url: "List/{searchString}/Page{page}",
            //   defaults: new { controller = "Offer", action = "List", category = (string)null }
            //);






            //routes.MapRoute(
            //    name: "ListRoute2",
            //    url: "{category}/{searchString}/Page{page}",
            //    defaults: new { controller = "Offer", action = "List" }
            //);
            //routes.MapRoute(
            //    name: null,
            //    url: "{category}/Page{page}",
            //    defaults: new { controller = "Offer", action = "List" }
            //);
            //routes.MapRoute(
            //    name: null,
            //    url: "Buy/{game}",
            //    defaults: new { controller = "Offer", action = "List" }
            //);


            routes.MapRoute(
                name: null,
                url: "game={game}",
                defaults: new { controller = "Offer", action = "OfferList", game = "csgo" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Offer", action = "List" }
            );
        

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{game}",
            //    defaults: new { controller = "Offer", action = "List", game = "csgo" }
            //);
            //routes.MapRoute(
            //    name: null,
            //    url: "myaccount/{controller}/{action}",
            //    defaults: new { controller = "Offer", action = "All" }
            //);
        }
    }
}
