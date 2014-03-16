using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using XPGroup.Models;

namespace System.Web.Mvc
{
    public static class AttributeDdlist
    {
        public static MvcHtmlString AttributeDropdownlist(this HtmlHelper htmlHelper, List<AttributeOption> attributeOptions, string name, string selectedValue = "", object htmlAttribute = null)
        {
            string atr = string.Empty;
            foreach (var item in HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttribute))
            {
                atr += item.Key + "=\"" + item.Value + "\" ";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<select id=" + name + " name=" + name + " " + atr + ">");
            foreach (var item in attributeOptions)
            {
                if (item.Value == selectedValue)
                {
                    sb.Append("<option value='" + item.Value + "' selected='selected'>" + item.Value + "</option>");
                }
                else
                {
                    sb.Append("<option value='" + item.Value + "'>" + item.Value + "</option>");
                }
            }
            sb.Append("</select>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString AttributeDropdownlistFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, List<AttributeOption> attributeOptions, string selectedValue = "", object htmlAttribute = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return AttributeDropdownlist(htmlHelper, attributeOptions, metadata.PropertyName, selectedValue, htmlAttribute);
        }
    }
}