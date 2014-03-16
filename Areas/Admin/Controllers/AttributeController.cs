using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class AttributeController : BaseController
    {
        public IAttributeRepository attributeRepository { get; set; }

        public IAttributeOptionRepository attributeOptionRepository { get; set; }

        public ICategoryRepository categoryRepository { get; set; }

        public AttributeController(IAttributeRepository attr, IAttributeOptionRepository attrOption, ICategoryRepository cat)
        {
            this.attributeRepository = attr;
            this.attributeOptionRepository = attrOption;
            this.categoryRepository = cat;
        }

        //
        // GET: /Admin/Attribute/

        public ActionResult Index()
        {
            return View(attributeRepository.GetAll());
        }

        //
        //Get: /Admin/Attribute/Add

        public ActionResult Add()
        {
            ViewBag.Categories = categoryRepository.GetParentChildCategory();
            return View();
        }

        //
        //Post: /Admin/Attribute/Add

        [HttpPost]
        public ActionResult Add(XPGroup.Models.Attribute attribute)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (attribute.CategoryId == -1)
                    {
                        List<Category> categories = categoryRepository.GetAll().ToList();
                        if (categories.Exists(c => c.ParentId == -1))
                        {
                            attribute.CategoryId = categories.Where(c => c.ParentId == -1).First().CategoryId;
                        }
                        else
                        {
                            Category category = new Category()
                            {
                                Name = "All category",
                                ParentId = -1,
                                SortOrder = 0
                            };
                            Category catReturn = categoryRepository.Add(category);
                            attribute.CategoryId = catReturn.CategoryId;
                        }
                    }
                    Attribute returnAttr = attributeRepository.Add(attribute);
                    var st = Request.Form["resultOption"].ToString();
                    st = st.TrimOneEndChar();
                    AttributeOption attrOption = new AttributeOption();
                    attrOption.AttributeId = returnAttr.AttributeId;
                    foreach (var item in st.Split(';'))
                    {
                        attrOption.Value = item;
                        attributeOptionRepository.Add(attrOption);
                    }
                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Add");
                }
            }
            else
                return RedirectToAction("Add");
        }

        //
        //Get: /Admin/Attribute/Edit/id
        public ActionResult Edit(int id)
        {
            ViewBag.Categories = categoryRepository.GetParentChildCategory();
            return View(attributeRepository.GetById(id));
        }

        //
        //Post: /Admin/Attribute/Edit
        [HttpPost]
        public ActionResult Edit(Attribute attribute)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (attribute.CategoryId == -1)
                    {
                        List<Category> categories = categoryRepository.GetAll().ToList();
                        if (categories.Exists(c => c.ParentId == -1))
                        {
                            attribute.CategoryId = categories.Where(c => c.ParentId == -1).First().CategoryId;
                        }
                        else
                        {
                            Category category = new Category()
                            {
                                Name = "All category",
                                ParentId = -1,
                                SortOrder = 0
                            };
                            Category catReturn = categoryRepository.Add(category);
                            attribute.CategoryId = catReturn.CategoryId;
                        }
                    }
                    attributeRepository.Update(attribute);

                    //delete all attribute options
                    if (attributeOptionRepository.DeleteAllByAttributeId(attribute.AttributeId))
                    {
                        var st = Request.Form["resultOption"].ToString();
                        st = st.TrimOneEndChar();
                        AttributeOption attrOption = new AttributeOption();
                        attrOption.AttributeId = attribute.AttributeId;
                        foreach (var item in st.Split(';'))
                        {
                            attrOption.Value = item;
                            attributeOptionRepository.Add(attrOption);
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Edit", new { id = attribute.AttributeId });
                }
            }
            return RedirectToAction("Edit", new { id = attribute.AttributeId });
        }

        //
        //Delete: /Admin/Attribute/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Attribute attribute = attributeRepository.GetById(id);
            if (attribute == null)
            {
                return Json("Delete attribute fail", JsonRequestBehavior.AllowGet);
            }
            else
            {
                attributeRepository.Delete(attribute);
                return Json("Delete attribute " + attribute.Name + " successful");
            }
        }
    }
}