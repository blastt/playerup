using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.ExtentionMethods
{
    public static class CurrencyExtentions
    {
        public static string GetStringPrice(this decimal price, string format, IFormatProvider formatProvider)
        {
            return string.Format(formatProvider,format, price);
        }
    }
}