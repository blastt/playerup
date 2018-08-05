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
            var offers = _offerService.GetOffersAsNoTracking(m => true, i => i.Game, i => i.FilterItems, i => i.FilterItems.Select(fi => fi.Filter), i => i.UserProfile).ToList();
            

            var model = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(offers);

            return View(model);
        }
    }
}