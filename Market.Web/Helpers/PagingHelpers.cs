﻿using Market.Web.ViewModels;
using System.Text;
using System.Web.Mvc;

namespace Market.Web.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PagedListPager(this HtmlHelper html,
                                              PageInfoViewModel pageInfo, string jsFunctionName)
        {
            
            StringBuilder result = new StringBuilder();
            TagBuilder div = new TagBuilder("div");
            
            div.MergeAttribute("id", "contentPager");
            div.AddCssClass("page-container");
            int p = 1;
            if (pageInfo.PageNumber >= 5)
            {
                TagBuilder tag = new TagBuilder("input");
                tag.MergeAttribute("value", "1");
                tag.MergeAttribute("type", "button");
                tag.AddCssClass("btn");
                tag.AddCssClass("btn-default");
                tag.MergeAttribute("onclick", jsFunctionName + "(" + 1 + ")");
                div.InnerHtml += tag;
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass(" mr-2");
                span.InnerHtml = ("...");
                div.InnerHtml += span;
                p = pageInfo.PageNumber - 3;
            }
            int to = pageInfo.TotalPages;
            bool lastBtn = false;
            if ((pageInfo.TotalPages - pageInfo.PageNumber) >= 4)
            {
                lastBtn = true;
                to = pageInfo.PageNumber + 3;
            }
            for (int i = p; i <= to; i++)
            {
                
                
                TagBuilder tag = new TagBuilder("input");
                tag.MergeAttribute("type", "button");
                tag.MergeAttribute("value", i.ToString());
                tag.AddCssClass("btn");
                if (i == pageInfo.PageNumber)
                {

                    tag.AddCssClass("btn-currentpage");
 
                    
                }
                else
                {
                    tag.AddCssClass("btn-default");
                    tag.MergeAttribute("onclick", jsFunctionName + "(" + i + ")");
                }
                
                div.InnerHtml += tag;
                
            }
            if (lastBtn)
            {
                TagBuilder tag = new TagBuilder("input");
                tag.MergeAttribute("value", pageInfo.TotalPages.ToString());
                tag.MergeAttribute("type", "button");
                tag.AddCssClass("btn");
                tag.AddCssClass("btn-default");
                tag.MergeAttribute("onclick", jsFunctionName + "(" + pageInfo.TotalPages + ")");
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass(" mr-2");
                span.InnerHtml = ("...");
                div.InnerHtml += span;
                div.InnerHtml += tag;
                
                
            }
            result.Append(div.ToString());
            return MvcHtmlString.Create(result.ToString());
        }
    }
}