using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ExtentionMethods
{
    public static class CurrencyExtentions
    {
        public static string GetStringPrice(this decimal price, string format, IFormatProvider provider)
        {
            return string.Format(provider,format, price);
        }
    }
}