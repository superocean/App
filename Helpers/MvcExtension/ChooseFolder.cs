using System.Collections.Generic;
using System.IO;
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
    public static class ChooseFolderClass
    {
        public static MvcHtmlString ChooseFolder(this HtmlHelper htmlHelper, string name, string selectedFolder, string value = "")
        {
            StringBuilder sb = new StringBuilder();
            string random = Helper.RandomInt(100000000, 999999999).ToString();
            sb.Append(Css());
            sb.Append(Js());
            sb.Append(GetListFolder(selectedFolder));
            sb.Append("<input type='hidden'" + (value == "" ? string.Empty : " value=" + value + "") + " name='" + name + "' type='text' id='txtFolderPath' />");
            sb.Append("<label><span>Selected Folder: </span><span id='lblFolderPath'>" + (value == "" ? "No folder is selected" : value) + "</span></label>");
            sb.Append("<span class='button' id='selectFolderButton'>Choose Folder</span>");
            return MvcHtmlString.Create(sb.ToString());
        }

        private static string Css()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style type='text/css'>");
            sb.Append(".treeview, .treeview ul {");
            sb.Append("	padding: 0;");
            sb.Append("	margin: 0;");
            sb.Append("	list-style: none;");
            sb.Append("}");
            sb.Append(".treeview ul {");
            sb.Append("	margin-top: 4px;");
            sb.Append("}");
            sb.Append(".treeview .hitarea {");
            sb.Append("	background: url(/scripts/treeview/images/treeview-default.gif) -64px -25px no-repeat;");
            sb.Append("	height: 16px;");
            sb.Append("	width: 16px;");
            sb.Append("	margin-left: -16px;");
            sb.Append("	float: left;");
            sb.Append("	cursor: pointer;");
            sb.Append("}");
            sb.Append("/* fix for IE6 */");
            sb.Append("* html .hitarea {");
            sb.Append("	display: inline;");
            sb.Append("	float:none;");
            sb.Append("}");
            sb.Append(".treeview li {");
            sb.Append("	margin: 0;");
            sb.Append("	padding: 3px 0pt 3px 16px;");
            sb.Append("}");
            sb.Append(".treeview a.selected {");
            sb.Append("	background-color: #eee;");
            sb.Append("}");
            sb.Append("#treecontrol { margin: 1em 0; display: none; }");
            sb.Append(".treeview li { background: url(/scripts/treeview/images/treeview-default-line.gif) 0 0 no-repeat; }");
            sb.Append(".treeview li.collapsable, .treeview li.expandable { background-position: 0 -176px; }");
            sb.Append(".treeview .expandable-hitarea { background-position: -80px -3px; }");
            sb.Append(".treeview li.last { background-position: 0 -1766px }");
            sb.Append(".treeview li.lastCollapsable, .treeview li.lastExpandable { background-image: url(/scripts/treeview/images/treeview-default.gif); }");
            sb.Append(".treeview li.lastCollapsable { background-position: 0 -111px }");
            sb.Append(".treeview li.lastExpandable { background-position: -32px -67px }");
            sb.Append(".treeview div.lastCollapsable-hitarea, .treeview div.lastExpandable-hitarea { background-position: 0; }");
            sb.Append(".filetree li { padding: 3px 0 2px 16px; }");
            sb.Append(".filetree span.folder, .filetree span.file { padding: 1px 0 1px 18px; display: block; }");
            sb.Append(".filetree span.folder { background: url(/scripts/treeview/images/folder.gif) 0 0 no-repeat; }");
            sb.Append(".filetree li.expandable span.folder { background: url(/scripts/treeview/images/folder-closed.gif) 0 0 no-repeat; }");
            sb.Append(".filetree span.file { background: url(/scripts/treeview/images/file.gif) 0 0 no-repeat; }");
            sb.Append(".filetree span:hover{cursor: pointer; color:#0092C9;}");
            sb.Append(".selected{color:#0092C9;text-decoration:underline;}");
            sb.Append("</style>");
            return sb.ToString();
        }

        private static string AddCss()
        {
            return "<link rel='stylesheet' type='text/css' href='/scripts/treeview/jquery.treeview.css'/>";
        }

        private static string Js()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script src=\"/scripts/treeview/jquery.treeview.js\" type=\"text/javascript\"></script>");

            sb.Append("<script type=\"text/javascript\">");
            sb.Append("    $(document).ready(function () {");
            sb.Append(@"$('#dialog_SelectFolder').dialog({
                            autoOpen: false,
                            height: 400,
                            width: 500,
                            modal: true,
                            buttons: {
                                Cancel: function() {
                                    $(this).dialog('close');
                                }
                            },
                            close: function() {
                                $(this).dialog('close');
                            }
                            });");
            sb.Append("$('#selectFolderButton').click(function() {$('#dialog_SelectFolder').dialog('open');});");

            sb.Append("var $treeview = $(\"#browser\").treeview({");
            sb.Append("animated: 'fast',");
            sb.Append("persist: 'currentfolder',");
            sb.Append("unique: true");
            sb.Append("});");
            sb.Append("$(\"span\", $treeview).unbind(\"click.treeview\");");

            sb.Append("    });");

            sb.Append("    function selectNode(event, nodeHtmlEl) {");
            sb.Append("        if ($.browser.msie) {");
            sb.Append("            window.event.cancelBubble = true;");
            sb.Append("        }");
            sb.Append("        if (event.stopPropagation) {");
            sb.Append("            event.stopPropagation();");
            sb.Append("        };");

            sb.Append("$('#txtFolderPath').val($(nodeHtmlEl).attr(\"id\"));");
            sb.Append("$('#lblFolderPath').text($(nodeHtmlEl).attr(\"id\"));");
            sb.Append("$('#dialog_SelectFolder').dialog('close');");
            sb.Append("    }");
            sb.Append("</script>");
            return sb.ToString();
        }

        private static string GetListFolder(string selectedFolder)
        {
            StringBuilder sb = new StringBuilder();
            DirectoryInfo oDir = new DirectoryInfo(Helper.MapPathFiles());

            sb.Append("<div id='dialog_SelectFolder'>");

            sb.Append("<ul id=\"browser\" class=\"filetree\">");
            sb.Append("<li><span>" + oDir.Name + "</span>");
            GetFolders(ref sb, Helper.MapPathFiles(), selectedFolder);
            sb.Append("</li>");
            sb.Append("</ul>");

            sb.Append("</div>");
            return sb.ToString();
        }

        private static string GetFolders(ref StringBuilder sb, string path, string selectedFolder)
        {
            DirectoryInfo oDir = new DirectoryInfo(path);
            sb.Append("<ul>");
            foreach (DirectoryInfo oDirSub in oDir.GetDirectories())
            {
                if (GetRelativePath(oDirSub.FullName) == selectedFolder)
                {
                    sb.AppendFormat("<li class=\"closed\"><span class=\"folder currentselected\" onclick=\"selectNode(event, this);\" id=\"{0}\">{1}</span>", GetRelativePath(oDirSub.FullName), oDirSub.Name);
                }
                else
                {
                    sb.AppendFormat("<li class=\"closed\"><span class=\"folder\" onclick=\"selectNode(event, this);\" id=\"{0}\">{1}</span>", GetRelativePath(oDirSub.FullName), oDirSub.Name);
                }

                if (oDirSub.GetDirectories().Length > 0)
                {
                    GetFolders(ref sb, oDirSub.FullName, selectedFolder);
                    sb.Append("</li>");
                }
                else
                {
                    sb.Append("</li>");
                }
            }
            sb.Append("</ul>");
            return sb.ToString();
        }

        private static string GetRelativePath(string absolutePath)
        {
            string relativePath = "\\" + absolutePath.Replace(Helper.MapPathFiles(), string.Empty);
            relativePath = relativePath.Replace('\\', '/').ToLower();
            return relativePath;
        }

        public static MvcHtmlString ChooseFolderFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<System.Func<TModel, TProperty>> expression, string selectedFolder) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string value = metadata.Model == null ? "" : Convert.ToString(metadata.Model, System.Globalization.CultureInfo.CurrentCulture);
            return ChooseFolder(htmlHelper, metadata.PropertyName, selectedFolder, value);
        }
    }
}