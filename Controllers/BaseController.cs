using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace XPGroup.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public static Logger logger = LogManager.GetCurrentClassLogger();

    }
}
