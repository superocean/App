using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public class ProductRepository : IProductRepository
    {
        private DbWeb db { get; set; }

        public ProductRepository(DbWeb dbweb)
        {
            this.db = dbweb;
        }

        public Product Add(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return product;
        }

        public Product Update(Product product)
        {
            db.Entry(product).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return product;
        }

        public Product Delete(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
            return product;
        }

        public List<Product> GetAll()
        {
            return db.Products.ToList();
        }

        public Product GetById(int id)
        {
            return db.Products.Find(id);
        }

        public List<ProductView> GetProductView()
        {
            List<ProductView> products = new List<ProductView>();
            foreach (var item in db.Products.OrderByDescending(p => p.Date).ToList())
            {
                products.Add(new ProductView()
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    CategoryName = item.Category.Name,
                    Date = item.Date,
                    Price = item.Price,
                    IsShowBanner = item.IsShowBanner,
                    IsShowHomePage = item.IsShowHomePage
                });
            }
            return products;
        }

        public ProductViews GetProductPaging(int pagesize, int index, string orderBy, bool sortOrderDesc, int categoryId = -1, string searchString = "")
        {
            #region products

            List<Product> products = new List<Product>();
            orderBy = orderBy.ToLower();
            searchString = searchString.ToLower();
            int totalProduct = 1;
            if (categoryId == -1)
            {
                #region category -1

                if (searchString == "")
                {
                    totalProduct = db.Products.Count();
                    if (sortOrderDesc)
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.OrderByDescending(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.OrderByDescending(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.OrderBy(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.OrderBy(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                }
                else
                {
                    totalProduct = db.Products.Where(c => c.Name.Contains(searchString)).Count();
                    if (sortOrderDesc)
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.Where(c => c.Name.Contains(searchString)).OrderBy(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.Where(c => c.Name.Contains(searchString)).OrderBy(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                }

                #endregion category -1
            }
            else
            {
                #region category !=-1

                if (searchString == "")
                {
                    totalProduct = db.Products.Where(c => c.CategoryId == categoryId).Count();
                    if (sortOrderDesc)
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.Where(c => c.CategoryId == categoryId).OrderByDescending(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.Where(c => c.CategoryId == categoryId).OrderByDescending(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.Where(c => c.CategoryId == categoryId).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.Where(c => c.CategoryId == categoryId).OrderBy(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.Where(c => c.CategoryId == categoryId).OrderBy(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.Where(c => c.CategoryId == categoryId).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                }
                else
                {
                    totalProduct = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).Count();
                    if (sortOrderDesc)
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                    else
                    {
                        switch (orderBy)
                        {
                            case "name":
                                products = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).OrderBy(c => c.Name).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            case "price":
                                products = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).OrderBy(c => c.Price).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;

                            default:
                                products = db.Products.Where(c => c.CategoryId == categoryId).Where(c => c.Name.Contains(searchString)).OrderByDescending(c => c.Date).Skip((index - 1) * pagesize).Take(pagesize).ToList();
                                break;
                        }
                    }
                }

                #endregion category !=-1
            }

            #endregion products

            List<ProductView> productviews = new List<ProductView>();
            foreach (var item in products)
            {
                productviews.Add(new ProductView()
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    CategoryName = item.Category.Name,
                    Date = item.Date,
                    Price = item.Price,
                    IsShowBanner = item.IsShowBanner,
                    IsShowHomePage = item.IsShowHomePage
                });
            }
            ProductViews productview = new ProductViews();
            productview.Products = productviews;
            productview.AllItemsCount = totalProduct;
            return productview;
        }
    }
}