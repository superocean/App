using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private ICategoryRepository categoryRepository { get; set; }

        public CategoryController(ICategoryRepository cat)
        {
            this.categoryRepository = cat;
        }

        //
        // GET: /Admin/Category/

        public ActionResult Index()
        {
            return View(categoryRepository.GetViewParentChild());
        }

        //
        // GET: /Admin/Category/Add
        public ActionResult Add()
        {
            ViewBag.Categories = categoryRepository.GetParentChildCategory();
            Category category = new Category() { SortOrder = 0 };
            return View(category);
        }

        //
        // POST: /Admin/Category/Add
        [HttpPost]
        public ActionResult Add(Category category)
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
        // GET: /Admin/Category/Edit/CategoryId
        public ActionResult Edit(int id)
        {
            Category category = categoryRepository.GetById(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Categories = categoryRepository.GetParentChildCategory(category.ParentId);
            return View(category);
        }

        //
        // POST: /Admin/Category/Edit
        [HttpPost]
        public ActionResult Edit(Category category)
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
        //POST: /Admin/Category/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Category category = categoryRepository.GetById(id);
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
        //POST: /Admin/Category/UpdatePosition
        [HttpPost]
        public ActionResult UpdatePosition(int id, int order)
        {
            Category category = categoryRepository.GetById(id);
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