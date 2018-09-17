using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOfferService _offerService;
        public HomeController(IOfferService offerService)
        {
            _offerService = offerService;
        }
        // GET: Home
        public ActionResult Index()
        {
            var offers = _offerService.GetOffersAsNoTracking(m => true, i => i.Game,i => i.Filters, i => i.FilterItems, i => i.FilterItems.Select(fi => fi.Filter), i => i.UserProfile).ToList();

            

            var model = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers).ToList();
            var filterDict = new Dictionary<Model.Models.Filter, FilterItem>();

            foreach (var offer in model)
            {
                if (offer.Filters.Count != offer.FilterItems.Count) continue;
                for (var i = 0; i < offer.Filters.Count; i++)
                {

                    filterDict.Add(offer.Filters[i], offer.FilterItems[i]);
                }
                offer.FilterFilterItem = filterDict;
                filterDict = new Dictionary<Model.Models.Filter, FilterItem>();
            }

            return View(model);
        }
    }
}