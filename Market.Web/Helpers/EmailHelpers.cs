using System.Web.Mvc;

namespace Market.Web.Helpers
{
    public static class EmailHelpers
    {
        public static MvcHtmlString ActivateForm(string text, string linkText, string hrefUrl)
        {
            //StringBuilder result = new StringBuilder();
            //result.Append(date.Day);
            //result.Append(" ");
            //result.Append((Monthes)date.Day);
            //result.Append(" ");
            //result.Append(date.Year);
            TagBuilder div = new TagBuilder("div");
            TagBuilder p = new TagBuilder("p");
            TagBuilder a = new TagBuilder("a");
            TagBuilder strong = new TagBuilder("strong");
            div.MergeAttribute("style", "text-align: center");
            p.SetInnerText(text);
            a.MergeAttribute("style", "background-color: #168de2; padding: 10px 30px; text-decoration:none ;font-size:14px; color:#ffffff;");
            a.MergeAttribute("href", hrefUrl);
            strong.SetInnerText(linkText);
            a.InnerHtml += strong;
            div.InnerHtml += p;
            div.InnerHtml += a;
            
            
            return new MvcHtmlString(div.ToString());
        }
    }
}