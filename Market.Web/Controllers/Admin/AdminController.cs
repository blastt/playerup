using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Panel()
        {
            return View();
        }
    }
}