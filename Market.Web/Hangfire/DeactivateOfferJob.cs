using Hangfire;
using Market.Service;
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

        public DeactivateOfferJob(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        [DisableConcurrentExecution(10 * 60)]
        public void Do(int itemId)
        {
            var offer = offerService.GetOffer(itemId);
            if (offer != null)
            {
                offerService.DeactivateOffer(offer, offer.UserProfileId);
            }
            offerService.SaveOffer();
        }
    }
}