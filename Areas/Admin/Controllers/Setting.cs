using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XPGroup.Models;
using XPGroup.Models.Repository;

namespace XPGroup.Areas.Admin.Controllers
{
    public class SettingController : BaseController
    {
        //
        // GET: /Admin/Setting/

        public ActionResult Index()
        {
            string xml = System.IO.File.ReadAllText(Helper.MapPathData() + "security.data").DecodeXml();
            Settings setting = (Settings)Helper.DeSerialize(xml, typeof(Settings));
            return View(setting);
        }

        //POST:/Admin/Setting/
        [HttpPost]
        public ActionResult Index(Settings setting)
        {
            try
            {
                string xml = Helper.Serialize(setting, true).EncodeXml();
                System.IO.File.WriteAllText(Helper.MapPathData() + "security.data", xml);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
                throw;
            }
        }
    }
}