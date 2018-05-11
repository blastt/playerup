using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hangfire
{
    public class DeactivateOfferJob : IJob
    {
        private readonly IOfferService offerService;

        public DeactivateOfferJob(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        public void Do(int itemId)
        {
            var offer = offerService.GetOffer(itemId);
            if (offer != null)
            {
                offerService.DeactivateOffer(offer, offer.UserProfileId);
            }
        }
    }
}