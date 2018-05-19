﻿using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IOfferService
    {
        IEnumerable<Offer> GetOffers();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Offer GetOffer(int id);
        void Delete(Offer offer);
        Task<Offer> GetOfferAsync(int id);
        Task<IEnumerable<Offer>> GetOffersAsync(Expression<Func<Offer, bool>> where);
        decimal CalculateMiddlemanPrice(decimal offerPrice);
        IEnumerable<Offer> SearchOffers(string game, string sort, ref bool isOnline, ref bool searchInDiscription,
            string searchString, ref int page, int pageSize,ref int totalItems, ref decimal minGamePrice, ref decimal maxGamePrice, ref decimal priceFrom, ref decimal priceTo);
        void CreateOffer(Offer offer);
        void UpdateOffer(Offer offer);
        bool DeactivateOffer(Offer offer, string currentUserId);
        void SaveOffer();
        Task SaveOfferAsync();
    }

    public class OfferService : IOfferService
    {
        private readonly IOfferRepository offersRepository;
        private readonly IUnitOfWork unitOfWork;

        public OfferService(IOfferRepository offersRepository, IUnitOfWork unitOfWork)
        {
            this.offersRepository = offersRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IOfferService Members

        public IEnumerable<Offer> GetOffers()
        {
            var offers = offersRepository.GetAll();
            return offers;
        }

        public async Task<IEnumerable<Offer>> GetOffersAsync(Expression<Func<Offer, bool>> where)
        {
            var offers = await offersRepository.GetManyAsync(where);
            return offers;
        }

        public bool DeactivateOffer(Offer offer, string currentUserId)
        {
            if (offer != null && offer.UserProfileId == currentUserId && offer.State == OfferState.active)
            {
                offer.State = OfferState.inactive;
                offer.DateDeleted = DateTime.Now;
                return true;
            }
            return false;
        }

        public Offer GetOffer(int id)
        {
            var offer = offersRepository.GetById(id);
            return offer;
        }

        public async Task<Offer> GetOfferAsync(int id)
        {
            var offer = await offersRepository.GetAsync(o => o.Id == id);
            return offer;
        }

        public void Delete(Offer offer)
        {
            offersRepository.Delete(offer);
        }

        public void CreateOffer(Offer offer)
        {
            offersRepository.Add(offer);
        }

        public void UpdateOffer(Offer offer)
        {
            offersRepository.Update(offer);
        }

        private IEnumerable<Offer> SearchOffersByGame(string game)
        {
            IEnumerable<Offer> offers;
            
                offers = offersRepository.GetAll().Where(m => m.Game.Value == game);
            
            return offers;
        }

        private IEnumerable<Offer> SearchOffersByPrice(IEnumerable<Offer> offers, ref decimal priceFrom, ref decimal priceTo, ref decimal minGamePrice,ref decimal maxGamePrice)
        {
            if (offers.Count() != 0)
            {
                minGamePrice = offers.Min(m => m.Price);

                maxGamePrice = offers.Max(m => m.Price);


                if (priceFrom == 0)
                {
                    priceFrom = minGamePrice;


                }
                if (priceTo == 0)
                {
                    priceTo = maxGamePrice;

                }

                decimal priceF = priceFrom;
                decimal priceT = priceTo;

                offers = from offer in offers
                         where offer.Price >= priceF &&
                                offer.Price <= priceT
                         select offer;
            }
            
            return offers;
        }

        private IEnumerable<Offer> SearchOffersBySearchString(IEnumerable<Offer> offers, string searchString, ref bool searchInDiscription)
        {
            if (searchInDiscription)
            {
                offers = offers.Where(o => o.Header.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower()) || o.Discription.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower()));
            }
            else
            {
                offers = offers.Where(o => o.Header.Replace(" ", "").ToLower().Contains(searchString.Replace(" ", "").ToLower()));
            }
            return offers;
        }

        private IEnumerable<Offer> SearchOffersByOnlineUser(IEnumerable<Offer> offers,ref  bool isOnline)
        {
            if (isOnline)
            {
                offers = offers.Where(o => o.UserProfile.IsOnline);
            }
            return offers;
        }

        private IEnumerable<Offer> SearchOffersByPage(IEnumerable<Offer> offers,ref int page, int pageSize, ref int totalItems)

        {
            totalItems = offers.Count();
            offers = offers.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return offers;
        }

        private IEnumerable<Offer> SortOffers(IEnumerable<Offer> offers, string sort)
        {
            switch (sort)
            {
                case "bestSeller":
                    {
                        offers = from offer in offers
                                 orderby (offer.UserProfile.PositiveFeedbackCount - offer.UserProfile.NegativeFeedbackCount) descending
                                 select offer;
                        break;
                    }
                case "priceDesc":
                    {
                        offers = from offer in offers
                                 orderby offer.Price descending
                                 select offer;
                        break;
                    }
                case "priceAsc":
                    {
                        offers = from offer in offers
                                 orderby offer.Price ascending
                                 select offer;
                        break;
                    }
                case "newestOffer":
                    {
                        offers = from offer in offers
                                 orderby offer.DateCreated descending
                                 select offer;
                        break;
                    }
                default:
                    break;
            }
            return offers;
        }

        public IEnumerable<Offer> SearchOffers(string game, string sort, ref bool isOnline, ref bool searchInDiscription,
            string searchString, ref int page, int pageSize,ref int totalItems, ref decimal minGamePrice, ref decimal maxGamePrice, ref decimal priceFrom, ref decimal priceTo)
        {
            IEnumerable<Offer> offers;
            offers = SearchOffersByGame(game);
            offers = offers.Where(o => o.State == OfferState.active);
            offers = SearchOffersByPrice(offers,ref priceFrom,ref priceTo,ref minGamePrice, ref maxGamePrice);
            offers = SearchOffersBySearchString(offers, searchString,ref searchInDiscription);
            offers = SearchOffersByOnlineUser(offers,ref isOnline);
            offers = SortOffers(offers, sort);
            return offers;
        }

        public decimal CalculateMiddlemanPrice(decimal offerPrice)
        {
            decimal middlemanPrice = 0;

            if (offerPrice < 3000)
            {
                middlemanPrice = 300;

            }
            else if (offerPrice < 15000)
            {
                middlemanPrice = offerPrice * Convert.ToDecimal(0.1);
            }
            else
            {
                middlemanPrice = 1500;
            }

            return middlemanPrice;
        }

        public void SaveOffer()
        {
            unitOfWork.Commit();
        }

        public async Task SaveOfferAsync()
        {
            await unitOfWork.CommitAsync();
        }

        #endregion

    }
}
