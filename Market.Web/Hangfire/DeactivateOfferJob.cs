using Hangfire;
using Market.Service;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Market.Web.Hangfire
{
    public class DeactivateOfferJob
    {
        private readonly IOfferService offerService;
        private readonly IIdentityMessageService identityMessageService;
        public DeactivateOfferJob(IOfferService offerService, IIdentityMessageService identityMessageService)
        {
            this.offerService = offerService;
            this.identityMessageService = identityMessageService;
        }

        [DisableConcurrentExecution(10 * 60)]
        public void Do(int itemId)
        {
            
            var offer = offerService.GetOffer(itemId);
            if (offer != null)
            {
                offerService.DeactivateOffer(offer, offer.UserProfileId);
                offerService.SaveOffer();
                identityMessageService.SendAsync(new IdentityMessage()
                {
                    Body = $"Здравствуйте {offer.UserProfile.Name}, ваше объявление {offer.Header} деактивировано",
                    Subject = "Ваше объявление деактивировано",
                    Destination = offer.UserProfile.ApplicationUser.Email
                }).Wait();
                
            }
            
        }
    }
}


