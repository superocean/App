using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IHtmlTextRepository
    {
        HtmlText GetByName(string name);

        IEnumerable<HtmlText> GetAll();

        HtmlText Add(HtmlText htmlText);

        HtmlText Update(HtmlText htmlText);

        HtmlText Delete(HtmlText htmlText);
    }
}