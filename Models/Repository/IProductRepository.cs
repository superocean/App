using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using XPGroup.Models.ViewModels;

namespace XPGroup.Models.Repository
{
    public interface IProductRepository
    {
        Product GetById(int id);

        List<Product> GetAll();

        Product Add(Product product);

        Product Update(Product product);

        Product Delete(Product product);

        List<ProductView> GetProductView();

        ProductViews GetProductPaging(int pagesize, int index, string orderBy, bool sortOrderDesc, int categoryId = -1, string searchString = "");
    }
}