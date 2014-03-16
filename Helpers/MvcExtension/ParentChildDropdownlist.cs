using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace System.Web.Mvc.Html
{
    public class SelectListItemParent : SelectListItem
    {
        public SelectListItemParent()
        {
            Childs = new List<SelectListItemParent>();
        }

        public List<SelectListItemParent> Childs { get; set; }
    }

    public static class ParentChildList
    {
        public static MvcHtmlString ParentChildDropdownlist(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItemParent> list, object htmlAttribute = null, string parentClass = "", string childClass = "", bool haveNotSelected = true, string haveNotSelectedText = "Not selected", bool isShowChildren = true)
        {
            string atr = string.Empty;
            foreach (var item in HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute))
            {
                atr += item.Key + "=\"" + item.Value + "\" ";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id=" + name + " name=" + name + " " + atr + ">");
            if (haveNotSelected)
            {
                sb.Append("<option" + (parentClass == "" ? string.Empty : " class=" + parentClass) + " value=\"-1\" selected=\"selected\">" + haveNotSelectedText + "</option>");
            }
            foreach (var item in list)
            {
                sb.Append("<option" + (parentClass == "" ? string.Empty : " class=" + parentClass) + " value=" + item.Value + (item.Selected ? " selected=\"selected\" " : string.Empty) + ">" + item.Text + "</option>");

                if (isShowChildren && item.Childs.Count > 0)
                {
                    foreach (var child in item.Childs)
                    {
                        sb.Append("<option" + (childClass == "" ? string.Empty : " class=" + childClass) + " value=" + child.Value + (child.Selected ? " selected=\"selected\" " : string.Empty) + ">" + child.Text + "</option>");
                    }
                }
            }
            sb.Append("</select>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ParentChildDropdownlistFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItemParent> list, object htmlAttribute = null, string parentClass = "", string childClass = "", bool haveNotSelected = true, string haveNotSelectedText = "Not selected", bool isShowChildren = true) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return ParentChildDropdownlist(htmlHelper, metadata.PropertyName, list, htmlAttribute, parentClass, childClass, haveNotSelected, haveNotSelectedText, isShowChildren);
        }
    }
}