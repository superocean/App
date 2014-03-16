using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class NewsCategoryController : BaseController
    {
        private INewsCategoryRepository categoryRepository { get; set; }

        public NewsCategoryController(INewsCategoryRepository cat)
        {
            this.categoryRepository = cat;
        }

        //
        // GET: /Admin/NewsCategory/

        public ActionResult Index()
        {
            return View(categoryRepository.GetViewParentChild());
        }

        //
        // GET: /Admin/NewsCategory/Add
        public ActionResult Add()
        {
            ViewBag.Categories = categoryRepository.GetParentChildNewsCategory();
            return View();
        }

        //
        // POST: /Admin/NewsCategory/Add
        [HttpPost]
        public ActionResult Add(NewsCategory category)
        {
            category.ParentId = category.ParentId == -1 ? 0 : category.ParentId;
            if (ModelState.IsValid)
            {
                try
                {
                    categoryRepository.Add(category);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("Add");
                }
            }
            return RedirectToAction("Add");
        }

        //
        // GET: /Admin/NewsCategory/Edit/NewsCategoryId
        public ActionResult Edit(int id)
        {
            NewsCategory category = categoryRepository.GetById(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Categories = categoryRepository.GetParentChildNewsCategory(category.ParentId);
            return View(category);
        }

        //
        // POST: /Admin/NewsCategory/Edit
        [HttpPost]
        public ActionResult Edit(NewsCategory category)
        {
            category.ParentId = category.ParentId == -1 ? 0 : category.ParentId;
            if (ModelState.IsValid)
            {
                try
                {
                    categoryRepository.Update(category);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return RedirectToAction("Edit", new { id = category.CategoryId });
                }
            }
            return RedirectToAction("Edit", new { id = category.CategoryId });
        }

        //
        //POST: /Admin/NewsCategory/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            NewsCategory category = categoryRepository.GetById(id);
            if (category == null)
            {
                return Json("Delete category fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                categoryRepository.Delete(category);
                return Json("Delete category " + category.Name + " successful");
            }
        }

        //
        //POST: /Admin/NewsCategory/UpdatePosition
        [HttpPost]
        public ActionResult UpdatePosition(int id, int order)
        {
            NewsCategory category = categoryRepository.GetById(id);
            if (category == null)
            {
                return Json("Update position category fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                category.SortOrder = order;
                categoryRepository.Update(category);
                return Json("Update position category " + category.Name + " successful");
            }
        }
    }
}