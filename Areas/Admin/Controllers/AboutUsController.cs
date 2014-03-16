using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class AboutUsController : BaseController
    {
        private IHtmlTextRepository htmlTextRepository { get; set; }

        public AboutUsController(IHtmlTextRepository ihtml)
        {
            this.htmlTextRepository = ihtml;
        }

        //
        // GET: /Admin/AboutUs/

        public ActionResult Index()
        {
            HtmlText text = htmlTextRepository.GetByName("AboutUs");
            return View(text);
        }

        //
        //POST: /Admin/AboutUs/
        [HttpPost]
        public ActionResult Index(HtmlText text)
        {
            if (ModelState.IsValid)
            {
                text.Content = HttpUtility.HtmlDecode(text.Content);
                htmlTextRepository.Update(text);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}