using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;

namespace XPGroup.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult AddCart()
        {
            ShoppingCart.Add(new Product() { Name = "card" });
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult PostData(List<Data> datas)
        {
            return Json("ok", JsonRequestBehavior.AllowGet);
        }
    }

    public class Data
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}