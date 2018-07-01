using Hangfire;
using Market.Service;
using Market.Web.Helpers;
using Microsoft.AspNet.Identity;

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
        public void Do(int itemId, string callbackUrl)
        {
            
            var offer = offerService.GetOffer(itemId);
            if (offer != null)
            {
                offerService.DeactivateOffer(offer, offer.UserProfileId);
                offerService.SaveOffer();
                string body = EmailHelpers.ActivateForm($"Здравствуйте {offer.UserProfile.Name}, ваше объявление {offer.Header} деактивировано.", "Активировать", callbackUrl).ToString();
                identityMessageService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = "Ваше объявление деактивировано",
                    
                    Destination = offer.UserProfile.ApplicationUser.Email
                }).Wait();
                
            }
            
        }
    }
}


