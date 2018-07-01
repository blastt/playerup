using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Helpers
{
    public static class OnlineHelpers
    {
        public static MvcHtmlString GetUserStatus(this HtmlHelper html, string userName, object forOnlineAttributes, object forOfflineAttributes)
        {
            TagBuilder span = new TagBuilder("span");
            if (HttpRuntime.Cache["LoggedInUsers"] != null)
            {
                var loggedOnUsers = HttpRuntime.Cache["LoggedInUsers"] as Dictionary<string, DateTime>;
                if (loggedOnUsers.Any(u => u.Key == userName))
                {
                    span.SetInnerText("Online");
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(forOnlineAttributes);
                    foreach (KeyValuePair<string, object> attribute in attributes)
                    {
                        span.MergeAttribute(attribute.Key.ToString(), attribute.Value.ToString());
                    }
                }
                else
                {
                    span.SetInnerText("Offline");
                    var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(forOfflineAttributes);
                    foreach (KeyValuePair<string, object> attribute in attributes)
                    {
                        span.MergeAttribute(attribute.Key.ToString(), attribute.Value.ToString());
                    }
                }
            }
            
            

            return MvcHtmlString.Create(span.ToString());
        }
    }
}