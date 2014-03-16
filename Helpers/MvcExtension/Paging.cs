using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using XPGroup.Areas.Admin.ViewModels;
using XPGroup.Models;

namespace System.Web.Mvc
{
    public static class Page
    {
        public static MvcHtmlString Paging(this HtmlHelper htmlHelper, int itemCount, int pageSize, int pageShow, string itemClass, string activeClass)
        {
            StringBuilder sb = new StringBuilder();
            string random = Helper.RandomInt(100000000, 999999999).ToString();
            int currentIndex = 1;
            try
            {
                currentIndex = HttpContext.Current.Request.QueryString["page"] == null ? 1 : Convert.ToInt32(HttpContext.Current.Request.QueryString["page"].ToString());
            }
            catch (Exception)
            {
                currentIndex = 1;
            }
            sb.Append(RenderPageLink(itemCount, currentIndex, pageSize, pageShow, itemClass, activeClass));
            return MvcHtmlString.Create(sb.ToString());
        }

        public static string RenderPageLink(int listCount, int index, int pageSize, int pageShow, string itemClass, string activeClass)
        {
            int pageCount = (double)listCount / pageSize < 1 ? 1 : (int)(Math.Ceiling((double)listCount / pageSize));
            int pageMid = pageShow / 2;

            int startPage = (index - 2) < 1 ? 1 : index - 2;
            int endPage = startPage + pageShow;
            if (startPage + pageShow > pageCount)
            {
                endPage = pageCount;
                startPage = (pageCount - pageShow) > 0 ? pageCount - pageShow : 1;
            }

            NameValueCollection nvc = new NameValueCollection();
            foreach (string key in HttpContext.Current.Request.QueryString) { if (key != "page") { nvc.Add(key, HttpContext.Current.Request.QueryString[key]); } }
            string qs = "";
            foreach (string key in nvc)
            {
                qs += qs == "" ? "?" : "&";
                qs += key + "=" + nvc[key];
            }

            string urlCurrentPage = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf('?') == -1 ? HttpContext.Current.Request.Url.AbsoluteUri : HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf('?'));
            string url = urlCurrentPage + qs;
            url = url.Contains('?') ? url + "&" : url + "?";
            StringBuilder sb = new StringBuilder();
            sb.Append("<table><tr>");
            sb.Append(index > 1 ? "<td class='" + itemClass + "'><a href='" + url + "page=" + (index - 1).ToString() + "'>Previous</a></td>" : "");
            if (startPage != endPage)
            {
                for (int i = startPage; i <= endPage; i++)
                {
                    sb.Append(index == i ? "<td class='" + activeClass + "'><span>" + i.ToString() + "</span></td>" : "<td class='" + itemClass + "'><a href='" + url + "page=" + i.ToString() + "'>" + i.ToString() + "</a></td>");
                }
            }

            sb.Append((pageCount > 1 && index != pageCount) ? "<td class='" + itemClass + "'><a href='" + url + "page=" + (index + 1).ToString() + "'>Next</a></td>" : "");
            sb.Append("</tr></table>");
            return sb.ToString();
        }
    }
}