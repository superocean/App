using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface INewsCategoryRepository
    {
        NewsCategory GetById(int id);

        IEnumerable<NewsCategory> GetAll();

        NewsCategory Add(NewsCategory newsCategory);

        NewsCategory Update(NewsCategory newsCategory);

        NewsCategory Delete(NewsCategory newsCategory);

        IEnumerable<NewsCategory> GetByParentId(int id);

        List<SelectListItemParent> GetParentChildNewsCategory(int selectedId = -1);

        List<ParentChildNewsCategory> GetViewParentChild();
    }
}