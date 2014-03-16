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
    public class ProductController : BaseController
    {
        public IProductRepository productRepository { get; set; }

        public ICategoryRepository categoryRepository { get; set; }

        public IAttributeRepository attributeRepository { get; set; }

        public IAttributeOptionRepository attributeOptionRepository { get; set; }

        public ProductController(IProductRepository product, ICategoryRepository category, IAttributeRepository attr, IAttributeOptionRepository attrOption)
        {
            this.productRepository = product;
            this.categoryRepository = category;
            this.attributeRepository = attr;
            this.attributeOptionRepository = attrOption;
        }

        //
        // GET: /Admin/Product/

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

            int sortOrder = 0;
            try
            {
                sortOrder = Request.QueryString["sortorder"] == null ? 0 : System.Convert.ToInt32(Request.QueryString["sortorder"].ToString());
            }
            catch (System.Exception)
            {
                sortOrder = 1;
            }
            bool sort = sortOrder == 1 ? true : false;

            string orderBy = "Date";
            try
            {
                orderBy = Request.QueryString["orderby"] == null ? "Date" : Request.QueryString["orderby"].ToString();
            }
            catch (System.Exception)
            {
                orderBy = "Date";
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
            List<SelectListItemParent> categories = categoryRepository.GetParentChildCategory(categoryId);
            ViewBag.Categories = categories;
            ProductViews products = productRepository.GetProductPaging(2, currentIndex, orderBy, sort, categoryId, search);
            return View(products);
        }

        //
        //GET: /Admin/Product/Add

        public ActionResult Add()
        {
            List<SelectListItemParent> categories = categoryRepository.GetParentChildCategory();
            ViewBag.Categories = categories;
            int selectedCategoryId = categories.Count > 0 ? System.Convert.ToInt32(categories[0].Value) : -1;
            List<Attribute> attributes = new List<Attribute>();
            foreach (var item in attributeRepository.GetAll().Where(c => c.Category.ParentId == -1 || c.CategoryId == selectedCategoryId))
            {
                attributes.Add(item);
            }
            ViewBag.Attributes = attributes;
            return View();
        }

        //
        //POST: /Admin/Product/Add

        [HttpPost]
        public ActionResult Add(Product product)
        {
            product.Date = System.DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    product.Date = System.DateTime.Now;
                    product.Content = HttpUtility.HtmlDecode(product.Content);
                    if (product.Image != null)
                    {
                        product.Image = product.Image.Replace(System.Helper.PathFiles(), string.Empty);
                    }

                    productRepository.Add(product);
                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Add");
                }
            }
            return RedirectToAction("Add");
        }

        //Post: Admin/Product/CategoryChange
        [HttpPost]
        public ActionResult CategoryChange(int id)
        {
            List<AttributeView> attributes = new List<AttributeView>();
            foreach (var item in attributeRepository.GetAll().Where(c => c.Category.ParentId == -1 || c.CategoryId == id))
            {
                AttributeView attr = new AttributeView();
                attr.Name = item.Name;
                attr.Value = item.Value;
                attr.Id = item.AttributeId;

                List<AttributeOptionView> attrOption = new List<AttributeOptionView>();
                foreach (var aOption in item.Attributes)
                {
                    attrOption.Add(new AttributeOptionView()
                    {
                        AttributeOptionId = aOption.AttributeOptionId,
                        Value = aOption.Value
                    });
                }
                attr.Attributes = attrOption;
                attributes.Add(attr);
            }

            return Json(attributes, JsonRequestBehavior.AllowGet);
        }

        //Get: Admin/Product/Edit/id
        public ActionResult Edit(int id)
        {
            Product product = productRepository.GetById(id);
            List<SelectListItemParent> categories = categoryRepository.GetParentChildCategory(product.CategoryId);
            ViewBag.Categories = categories;
            int selectedCategoryId = categories.Count > 0 ? System.Convert.ToInt32(product.CategoryId) : -1;

            ViewBag.AttributesSaved = product.Attributes;

            List<Attribute> attributes = new List<Attribute>();
            foreach (var item in attributeRepository.GetAll().Where(c => c.Category.ParentId == -1 || c.CategoryId == selectedCategoryId))
            {
                attributes.Add(item);
            }

            ViewBag.Attributes = attributes;
            return View(product);
        }

        //
        //POST: /Admin/Product/Edit

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.Content = HttpUtility.HtmlDecode(product.Content);
                    if (product.Image != null)
                    {
                        product.Image = product.Image.Replace(System.Helper.PathFiles(), string.Empty);
                    }
                    productRepository.Update(product);
                    return RedirectToAction("Index");
                }
                catch (System.Exception)
                {
                    return RedirectToAction("Edit", new { id = product.ProductId });
                }
            }
            return RedirectToAction("Edit", new { id = product.ProductId });
        }

        //
        //Post: /Admin/Product/Delete
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = productRepository.GetById(id);
            try
            {
                if (product == null)
                {
                    return Json("Delete product fail", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    productRepository.Delete(product);
                    return Json("Delete product " + product.Name + " successful");
                }
            }
            catch (System.Exception)
            {
                return Json("Delete product fail", JsonRequestBehavior.AllowGet);
            }
        }

        //
        //Post /Admin/Product/UpdateProductView
        [HttpPost]
        public ActionResult UpdateProductView(int id, bool isShowHomePage, bool isShowBanner)
        {
            Product product = productRepository.GetById(id);
            try
            {
                if (product == null)
                {
                    return Json("Update fail", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    product.IsShowHomePage = isShowHomePage;
                    product.IsShowBanner = isShowBanner;
                    productRepository.Update(product);
                    return Json("Update product " + product.Name + " successful");
                }
            }
            catch (System.Exception)
            {
                return Json("Update fail", JsonRequestBehavior.AllowGet);
            }
        }

        //
        //Post /Admin/Product/ResetQueryString
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