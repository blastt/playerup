using Market.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Market.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //JobHelper.SetSerializerSettings(new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            var loggedInUsers = new Dictionary<string, DateTime>();
            HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;

            Bootstrapper.Run();
        }

        protected void Application_AuthorizeRequest()
        {
            Response.AppendHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("5ac35eb73b1eaf90618b456b:49YehvH0IJr19yCNtmWxBR3TbuwKCCaN")));
        }

        protected void Application_EndRequest()
        {
            //var db = new MarketEntities();
            
            var loggedInUsers = (Dictionary<string, DateTime>)HttpRuntime.Cache["LoggedInUsers"];

            if (User != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userName = User.Identity.Name;
                    if (loggedInUsers != null)
                    {
                        loggedInUsers[userName] = DateTime.Now;
                        HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                    }
                }

                if (loggedInUsers != null)
                {
                    foreach (var item in loggedInUsers.ToList())
                    {
                        if (item.Value < DateTime.Now.AddMinutes(-10))
                        {
                            loggedInUsers.Remove(item.Key);
                        }
                    }
                    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                }
                //else
                //{
                //    loggedInUsers = new Dictionary<string, DateTime>();
                //    //add this user to the list
                //    loggedInUsers.Add(User.Identity.Name, DateTime.Now);
                //    //add the list into the cache
                //    HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                //}
            }



        }

    }
}
