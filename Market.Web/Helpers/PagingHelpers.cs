using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PagedListPager(this HtmlHelper html,
                                              PageInfoViewModel pageInfo,
                                              Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder div = new TagBuilder("div");
            div.MergeAttribute("id", "contentPager");
            div.AddCssClass("page-container");
            for (int i = 1; i <= pageInfo.TotalPages; i++)
            {
                
                
                TagBuilder tag = new TagBuilder("btn");
                tag.MergeAttribute("value", i.ToString());
                tag.AddCssClass("btn");
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.PageNumber)
                {

                    tag.AddCssClass("btn-currentpage");
 
                    
                }
                else
                {
                    tag.AddCssClass("btn-default");
                    tag.MergeAttribute("onclick", "SelectPage(" + i + ")");
                }
                
                div.InnerHtml += tag;
                
            }
            result.Append(div.ToString());
            return MvcHtmlString.Create(result.ToString());
        }
    }
}