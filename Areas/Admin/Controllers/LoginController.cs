using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;

namespace XPGroup.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Admin/Login/

        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        //POST:/Admin/Login/
        [HttpPost]
        public ActionResult Index(AdminUser user)
        {
            //check user
            string xml = System.IO.File.ReadAllText(Helper.MapPathData() + "security.data").DecodeXml();
            Settings setting = (Settings)Helper.DeSerialize(xml, typeof(Settings));
            if (user.Username == setting.AdminUser.Username && user.Password == setting.AdminUser.Password)
            {
                System.Web.HttpContext.Current.Session.Add("userauthorized", true);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return RedirectToAction("Index", "Login", new { message = "Username or Password incorrect" });
            }
        }
    }
}