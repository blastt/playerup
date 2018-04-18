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

            

            routes.MapRoute(
                    name: null,
                    url: "account/dialog/{dialogId}",
                    defaults: new { controller = "Dialog", action = "Details", dialogId = "" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/order/sell/{orderId}",
                    defaults: new { controller = "Order", action = "SellDetails", orderId = "" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/order/buy/{orderId}",
                    defaults: new { controller = "Order", action = "BuyDetails", orderId = "" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/messagess/inbox",
                    defaults: new { controller = "Message", action = "Inbox" }
                );
            routes.MapRoute(
                    name: null,
                    url: "account/messagess/unread",
                    defaults: new { controller = "Message", action = "Unread" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/offers/active",
                    defaults: new { controller = "Offer", action = "Active" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/offers/inactive",
                    defaults: new { controller = "Offer", action = "Inactive" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/offers/closed",
                    defaults: new { controller = "Offer", action = "Closed" }
                );

            routes.MapRoute(
                   name: null,
                   url: "account/orders/buy",
                   defaults: new { controller = "Order", action = "OrderBuy" }
               );

            routes.MapRoute(
                    name: null,
                    url: "account/orders/sell",
                    defaults: new { controller = "Order", action = "OrderSell" }
                );


            routes.MapRoute(
                    name: null,
                    url: "account/feedbacks/all",
                    defaults: new { controller = "Feedback", action = "All" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/feedbacks/positive",
                    defaults: new { controller = "Feedback", action = "Positive" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/feedbacks/negative",
                    defaults: new { controller = "Feedback", action = "Negative" }
                );

            routes.MapRoute(
                    name: null,
                    url: "account/settings",
                    defaults: new { controller = "Account", action = "settings" }
                );

            



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
                url: "game/{game}",
                defaults: new { controller = "Offer", action = "Buy", game = "csgo" }
            );

            routes.MapRoute(
                    name: null,
                    url: "user/{userName}",
                    defaults: new { controller = "Profile", action = "Info", userName = "" }
                );

            routes.MapRoute(
                    name: null,
                    url: "faq",
                    defaults: new { controller = "help", action = "faq" }
                );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Offer", action = "Buy" }
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
