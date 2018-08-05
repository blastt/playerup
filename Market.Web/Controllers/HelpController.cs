using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Threading.Tasks;
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

        [HttpGet]
        public ActionResult PublicOffer()
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
                try
                {
                    // Create a Web transport for sending email.
                    var apiKey = ConfigurationManager.AppSettings["SENDGRID_KEY"];
                    var client = new SendGridClient(apiKey);
                    var to = new EmailAddress("support@playerup.ru", "PlayerUp");
                    var subject = message.Subject;

                    var from = new EmailAddress(message.Destination);
                    var plainTextContent = message.Body;
                    var htmlContent = message.Body;
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);
                }
                catch (Exception)
                {


                }
                return View("MessageSendSuccess");
            }
            return HttpNotFound();
        }
    }
}