using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class HelpController : Controller
    {
        
        private readonly IIdentityMessageService _identityMessageService;

        public HelpController(IIdentityMessageService identityMessageService)
        {
            _identityMessageService = identityMessageService;
        }
        // GET: Help
        public ActionResult Faq()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ContactUs(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityMessage message = new IdentityMessage
                {
                    Destination = model.Email,
                    Body = model.MessageBody,
                    Subject = model.MessageSubject
                };
                await _identityMessageService.SendAsync(message);
                return View();
            }
            return HttpNotFound();
        }
    }
}