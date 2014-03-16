using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class ProductAttributeController : BaseController
    {
        //
        // GET: /Admin/ProductAttribute/

        public ActionResult Index()
        {
            return View();
        }
    }
}