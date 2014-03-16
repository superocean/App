using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Script.Serialization;
using XPGroup.Areas.Admin.ViewModels;
using XPGroup.Models;
using XPGroup.Models.Repository;
using XPGroup.Models.ViewModels;

namespace XPGroup.Areas.Admin.Controllers
{
    public class ManageNewsController : BaseController
    {
        public INewsRepository newsRepository { get; set; }

        public INewsCategoryRepository categoryRepository { get; set; }

        public ManageNewsController(INewsRepository news, INewsCategoryRepository category)
        {
            this.newsRepository = news;
            this.categoryRepository = category;
        }

        //
        // GET: /Admin/News/

        public ActionResult Index()
        {
            int currentIndex = 1;
            try
            {
                currentIndex = Request.QueryString["page"] == null ? 1 : System.Convert.ToInt32(Request.QueryString["page"].ToString());
            }
            catch (System.Exception)
            {
                currentIndex = 1;
            }

            int categoryId = -1;
            try
            {
                categoryId = Request.QueryString["catid"] == null ? -1 : System.Convert.ToInt32(Request.QueryString["catid"].ToString());
            }
            catch (System.Exception)
            {
                categoryId = -1;
            }

            string search = "";
            try
            {
                search = Request.QueryString["q"] == null ? "" : Request.QueryString["q"].ToString();
            }
            catch (System.Exception)
            {
                search = "";
            }
            List<SelectListItemParent> categories = categoryRepository.GetParentChildNewsCategory(categoryId);
            ViewBag.Categories = categories;
            NewsViews newss = newsRepository.GetNewsPaging(2, currentIndex, categoryId, search);
            return View(newss);
        }

        //
        //GET: /Admin/News/Add

        public ActionResult Add()
        {
            List<SelectListItemParent> categories = categoryRepository.GetParentChildNewsCategory();
            ViewBag.Categories = categories;
            int selectedNewsCategoryId = categories.Count > 0 ? System.Convert.ToInt32(categories[0].Value) : -1;
            return View();
        }

        //
        //POST: /Admin/News/Add

        [HttpPost]
        public ActionResult Add(News news)
        {
            news.Date = System.DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    news.Date = System.DateTime.Now;
                    news.Content = HttpUtility.HtmlDecode(news.Content);
                    if (news.Image != null)
                    {
                        news.Image = news.Image.Replace(System.Helper.PathFiles(), string.Empty);
                    }

                    newsRepository.Add(news);
                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Add");
                }
            }
            return RedirectToAction("Add");
        }

        //Get: Admin/News/Edit/id
        public ActionResult Edit(int id)
        {
            News news = newsRepository.GetById(id);
            List<SelectListItemParent> categories = categoryRepository.GetParentChildNewsCategory(news.CategoryId);
            ViewBag.Categories = categories;
            int selectedNewsCategoryId = categories.Count > 0 ? System.Convert.ToInt32(news.CategoryId) : -1;
            return View(news);
        }

        //
        //POST: /Admin/News/Edit

        [HttpPost]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    news.Content = HttpUtility.HtmlDecode(news.Content);
                    if (news.Image != null)
                    {
                        news.Image = news.Image.Replace(System.Helper.PathFiles(), string.Empty);
                    }
                    newsRepository.Update(news);
                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Edit", new { id = news.NewsId });
                }
            }
            return RedirectToAction("Edit", new { id = news.NewsId });
        }

        //
        //Post: /Admin/News/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            News news = newsRepository.GetById(id);
            try
            {
                if (news == null)
                {
                    return Json("Delete news fail", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    newsRepository.Delete(news);
                    return Json("Delete news " + news.Title + " successful");
                }
            }
            catch (System.Exception)
            {
                return Json("Delete news fail", JsonRequestBehavior.AllowGet);
            }
        }

        //
        //Post /Admin/News/ResetQueryString
        [HttpPost]
        public ActionResult ResetQueryString(string url, string queryString)
        {
            if (url.Contains('?'))
            {
                string[] arrQueryString = queryString.Split(';');
                string returnUrl = url.Substring(0, url.IndexOf('?'));
                string qstr = url.Substring(url.IndexOf('?') + 1, url.Length - url.IndexOf('?') - 1);
                string[] arr = qstr.Split('&');
                List<string> listQuery = new List<string>();
                for (int i = 0; i < arr.Length; i++)
                {
                    var check = true;
                    for (int j = 0; j < arrQueryString.Length; j++)
                    {
                        if (arr[i].Trim().Substring(0, arr[i].IndexOf('=')).ToLower() == arrQueryString[j])
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check && arr[i].Trim().Substring(0, arr[i].IndexOf('=')).ToLower() != "page")
                    {
                        listQuery.Add(arr[i]);
                    }
                }
                if (listQuery.Count > 0)
                {
                    qstr = listQuery[0];
                    for (int i = 1; i < listQuery.Count; i++)
                    {
                        qstr += "&" + listQuery[i];
                    }
                    returnUrl = returnUrl + "?" + qstr;
                }

                return Json(returnUrl, JsonRequestBehavior.AllowGet);
            }

            return Json(url, JsonRequestBehavior.AllowGet);
        }
    }
}