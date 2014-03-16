using System.Collections.Generic;
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
    public static class ProductAttributesDdl
    {
        public static MvcHtmlString ProductAttributesDdls(this HtmlHelper htmlHelper, string name, List<XPGroup.Models.Attribute> attributes, string attributeSaveds = "", string pClass = "attribute_Product", string selectClass = "dropdownlist selectAttribute_option")
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            List<AttributeSaved> attributesSaved = new List<AttributeSaved>();
            if (attributeSaveds != "")
            {
                attributesSaved = json_serializer.Deserialize<List<AttributeSaved>>(attributeSaveds);
            }

            StringBuilder sb = new StringBuilder();
            attributes = attributes ?? new List<XPGroup.Models.Attribute>();
            foreach (var item in attributes)
            {
                sb.Append("<p class=\"" + pClass + "\">");
                sb.Append("<input type=\"hidden\" value=\"" + item.AttributeId.ToString() + "\" />");
                sb.Append("	<label>" + item.Name + "</label>");
                sb.Append("	<select class=\"" + selectClass + "\">");

                //check have attribute saved
                bool check = attributesSaved.Exists(c => c.Id == item.AttributeId);
                if (check)
                {
                    AttributeSaved atr = attributesSaved.Find(c => c.Id == item.AttributeId);
                    foreach (var option in item.Attributes)
                    {
                        sb.Append("<option" + ((option.Value.ToLower().Trim() == atr.Value.ToLower().Trim()) ? " selected=\"selected\"" : string.Empty) + " value=\"" + option.Value + "\">" + option.Value + "</option>");
                    }
                }
                else
                {
                    foreach (var option in item.Attributes)
                    {
                        sb.Append("<option value=\"" + option.Value + "\">" + option.Value + "</option>");
                    }
                }

                sb.Append("	</select>");
                sb.Append("</p>");
            }
            sb.Append("<input type=\"hidden\" id=\"attribute_Product_saved\" name=\"" + name + "\" />");

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ProductAttributesDdlsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TProperty>> expression, List<XPGroup.Models.Attribute> attributes, string attributeSaveds = "", string pClass = "attribute_Product", string selectClass = "dropdownlist selectAttribute_option") where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return ProductAttributesDdls(htmlHelper, metadata.PropertyName, attributes, attributeSaveds, pClass, selectClass);
        }
    }
}