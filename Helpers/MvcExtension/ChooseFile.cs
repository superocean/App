using System.Collections.Generic;
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
    public static class Choose
    {
        public static MvcHtmlString ChooseFile(this HtmlHelper htmlHelper, string name, string value = "")
        {
            StringBuilder sb = new StringBuilder();
            string random = Helper.RandomInt(100000000, 999999999).ToString();
            sb.Append("<input type='hidden' value='" + value + "' id='image_" + random + "' name='" + name + "' />");
            sb.Append("<div style='margin-bottom: 10px' id='productPictureShow_" + random + "'>");
            sb.Append(value == "" ? "" : "<img src='" + Helper.PathFiles() + value + "' alt='' width=\"100\" height=\"100\"/>");
            sb.Append("</div>");
            sb.Append("<div onclick='browserServerCKE_" + random + "();' class='button'>Choose picture</div>");
            sb.Append("<script type='text/javascript'>");
            sb.Append("function browserServerCKE_" + random + "()");
            sb.Append("{");
            sb.Append("var finder = new CKFinder();");
            sb.Append("finder.basePath = '/Content/ckfinder';");
            sb.Append("finder.selectActionFunction = SetFileField_" + random + ";");
            sb.Append("finder.popup();");
            sb.Append("}");
            sb.Append("function SetFileField_" + random + "(fileUrl)");
            sb.Append("{");
            sb.Append("$('#image_" + random + "').val(fileUrl);");
            sb.Append("$('#productPictureShow_" + random + "').html('<img src=\"' + fileUrl + '\" width=\"100\" height=\"100\" alt=\"\"/>');");
            sb.Append("}");
            sb.Append("</script>");
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString ChooseFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TProperty>> expression) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = metadata.Model == null ? "" : Convert.ToString(metadata.Model, CultureInfo.CurrentCulture);
            return ChooseFile(htmlHelper, metadata.PropertyName, value);
        }
    }
}