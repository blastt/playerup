using Hangfire;
using Market.Service;
using Market.Web.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hangfire
{
    public class SendEmailAccountData
    {
        private readonly IOrderService orderService;
        private readonly IIdentityMessageService identityMessageService;
        public SendEmailAccountData(IOrderService orderService, IIdentityMessageService identityMessageService)
        {
            this.orderService = orderService;
            this.identityMessageService = identityMessageService;
        }

        [DisableConcurrentExecution(10 * 60)]
        public void Do(string login, string password, string email, string emailPassword, string additionalInfo, string userEmail)
        {
            //Url.Action("BuyDetails", "Order", new { id = offer.Order.Id }, protocol: Request.Url.Scheme)).ToString()
            string body = EmailHelpers.AccountData(login, password, email, emailPassword, additionalInfo).ToString();

            identityMessageService.SendAsync(new IdentityMessage()
            {
                Body = body,
                Subject = "Данные от аккаунта",

                Destination = userEmail
            }).Wait();
        }
    }
}