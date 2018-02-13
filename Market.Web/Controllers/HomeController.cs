using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
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

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(CreateOfferViewModel model)
        {
            var offer = Mapper.Map<CreateOfferViewModel, Offer>(model);
            offer.UserProfileId = User.Identity.GetUserId();
            _offerService.CreateOffer(offer);
            _offerService.SaveOffer();
            return View();
        }

        [Authorize]
        public ActionResult Details()
        {
            var offer = _offerService.GetOffers().FirstOrDefault();
            if(offer != null)
            {
                var detailsOfferViewModel = Mapper.Map<Offer, DetailsOfferViewModel>(offer);

                return View(detailsOfferViewModel);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}