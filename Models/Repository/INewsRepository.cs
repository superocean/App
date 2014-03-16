using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface INewsRepository
    {
        News GetById(int id);

        List<News> GetAll();

        News Add(News news);

        News Update(News news);

        News Delete(News news);

        List<NewsView> GetNewsView();

        NewsViews GetNewsPaging(int pagesize, int index, int categoryId = -1, string searchString = "");
    }
}