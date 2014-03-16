using System.Web.Mvc;

namespace XPGroup.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "LoginMessage",
               "Admin/Login/Index/{message}",
               new { controller = "Login", action = "Index", message = UrlParameter.Optional }
           );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "category", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}