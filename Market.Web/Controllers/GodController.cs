using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class GodController : Controller
    {
        public GodController()
        {

        }
        // GET: God
        public ActionResult AddFilter()
        {
            return View();
        }

        public ActionResult AddFilterItem()
        {
            return View();
        }

        
    }
}