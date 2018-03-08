using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Helpers
{
    public static class DateHelpers
    {
        public static MvcHtmlString Date(this HtmlHelper html, DateTime date)
        {
            //StringBuilder result = new StringBuilder();
            //result.Append(date.Day);
            //result.Append(" ");
            //result.Append((Monthes)date.Day);
            //result.Append(" ");
            //result.Append(date.Year);

            string result = $"{date.Day} {date.ToString("MMM", CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.ToString()))}. {date.Year}";
            return new MvcHtmlString(result.ToString());
        }
    }
}