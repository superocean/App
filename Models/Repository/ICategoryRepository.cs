using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface ICategoryRepository
    {
        Category GetById(int id);

        IEnumerable<Category> GetAll();

        Category Add(Category category);

        Category Update(Category category);

        Category Delete(Category category);

        IEnumerable<Category> GetByParentId(int id);

        List<SelectListItemParent> GetParentChildCategory(int selectedId = -1);

        List<ParentChildCategory> GetViewParentChild();
    }
}