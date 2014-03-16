using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class NewsCategoryRepository : INewsCategoryRepository
    {
        private DbWeb db { get; set; }

        public NewsCategoryRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public NewsCategory Add(NewsCategory category)
        {
            db.NewsCategories.Add(category);
            db.SaveChanges();
            return category;
        }

        public NewsCategory Update(NewsCategory category)
        {
            db.Entry(category).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return category;
        }

        public NewsCategory Delete(NewsCategory category)
        {
            db.NewsCategories.Remove(category);
            db.SaveChanges();
            return category;
        }

        public IEnumerable<NewsCategory> GetAll()
        {
            return db.NewsCategories.ToList();
        }

        public NewsCategory GetById(int id)
        {
            return db.NewsCategories.Find(id);
        }

        public IEnumerable<NewsCategory> GetByParentId(int id)
        {
            return db.NewsCategories.Where(c => c.ParentId == id).ToList();
        }

        public List<SelectListItemParent> GetParentChildNewsCategory(int selectedId = -1)
        {
            List<SelectListItemParent> list = new List<SelectListItemParent>();
            foreach (var parent in db.NewsCategories.Where(c => c.ParentId == 0).ToList())
            {
                SelectListItemParent itemParent = new SelectListItemParent();
                itemParent.Text = parent.Name;
                itemParent.Value = parent.CategoryId.ToString();
                itemParent.Selected = parent.CategoryId == selectedId ? true : false;
                List<NewsCategory> children = db.NewsCategories.Where(c => c.ParentId == parent.CategoryId).ToList();
                if (children.Count > 0)
                {
                    List<SelectListItemParent> childlist = new List<SelectListItemParent>();
                    foreach (var child in children)
                    {
                        SelectListItemParent itemChild = new SelectListItemParent();
                        itemChild.Text = child.Name;
                        itemChild.Value = child.CategoryId.ToString();
                        itemChild.Selected = child.CategoryId == selectedId ? true : false;
                        childlist.Add(itemChild);
                    }
                    itemParent.Childs = childlist;
                }
                list.Add(itemParent);
            }
            return list;
        }

        public List<ParentChildNewsCategory> GetViewParentChild()
        {
            List<ParentChildNewsCategory> listcategory = new List<ParentChildNewsCategory>();
            foreach (var parent in db.NewsCategories.Where(c => c.ParentId == 0).OrderBy(c => c.SortOrder).ToList())
            {
                ParentChildNewsCategory category = new ParentChildNewsCategory();
                category.CategoryId = parent.CategoryId;
                category.Name = parent.Name;
                category.Description = parent.Description;
                category.SortOrder = parent.SortOrder;
                category.News = parent.NewsList.ToList();
                foreach (var child in db.NewsCategories.Where(c => c.ParentId == parent.CategoryId).OrderBy(c => c.SortOrder).ToList())
                {
                    category.ChildCategories.Add(new ParentChildNewsCategory()
                    {
                        CategoryId = child.CategoryId,
                        Name = child.Name,
                        Description = child.Description,
                        News = child.NewsList.ToList(),
                        SortOrder = child.SortOrder
                    });
                }
                listcategory.Add(category);
            }
            return listcategory;
        }
    }
}