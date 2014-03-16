using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XPGroup.Models.Repository
{
    public class BaseController : System.Web.Mvc.Controller
    {
        public BaseController()
        {
            if (System.Web.HttpContext.Current.Session["userauthorized"] == null)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Admin/Login");
            }
        }
    }
}