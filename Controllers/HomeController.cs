using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using NLog;

namespace XPGroup.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            logger.Trace("Sample trace message");
            logger.Debug("Sample debug message");
            logger.Info("Sample informational message");
            logger.Warn("Sample warning message");
            logger.Error("Sample error message");
            logger.Fatal("Sample fatal error message");

            // alternatively you can call the Log() method 
            // and pass log level as the parameter.
            logger.Log(LogLevel.Info, "Sample informational message");
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