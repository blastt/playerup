using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {          
            return View();
        }

        //public ActionResult Forbidden()
        //{
        //    Response.StatusCode = 403;
        //    return View();
        //}
    }
}