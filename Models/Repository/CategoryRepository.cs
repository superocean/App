using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private DbWeb db { get; set; }

        public CategoryRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public Category Add(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return category;
        }

        public Category Update(Category category)
        {
            db.Entry(category).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return category;
        }

        public Category Delete(Category category)
        {
            db.Categories.Remove(category);
            db.SaveChanges();
            return category;
        }

        public IEnumerable<Category> GetAll()
        {
            return db.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return db.Categories.Find(id);
        }

        public IEnumerable<Category> GetByParentId(int id)
        {
            return db.Categories.Where(c => c.ParentId == id).ToList();
        }

        public List<SelectListItemParent> GetParentChildCategory(int selectedId = -1)
        {
            List<SelectListItemParent> list = new List<SelectListItemParent>();
            foreach (var parent in db.Categories.Where(c => c.ParentId == 0).ToList())
            {
                SelectListItemParent itemParent = new SelectListItemParent();
                itemParent.Text = parent.Name;
                itemParent.Value = parent.CategoryId.ToString();
                itemParent.Selected = parent.CategoryId == selectedId ? true : false;
                List<Category> children = db.Categories.Where(c => c.ParentId == parent.CategoryId).ToList();
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

        public List<ParentChildCategory> GetViewParentChild()
        {
            List<ParentChildCategory> listcategory = new List<ParentChildCategory>();
            foreach (var parent in db.Categories.Where(c => c.ParentId == 0).OrderBy(c => c.SortOrder).ToList())
            {
                ParentChildCategory category = new ParentChildCategory();
                category.CategoryId = parent.CategoryId;
                category.Name = parent.Name;
                category.Description = parent.Description;
                category.SortOrder = parent.SortOrder;
                category.Products = parent.Products.ToList();
                foreach (var child in db.Categories.Where(c => c.ParentId == parent.CategoryId).OrderBy(c => c.SortOrder).ToList())
                {
                    category.ChildCategories.Add(new ParentChildCategory()
                    {
                        CategoryId = child.CategoryId,
                        Name = child.Name,
                        Description = child.Description,
                        Products = child.Products.ToList(),
                        SortOrder = child.SortOrder
                    });
                }
                listcategory.Add(category);
            }
            return listcategory;
        }
    }
}