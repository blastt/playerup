using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IOfferService
    {
        IEnumerable<Offer> GetOffers();
        IQueryable<Offer> GetOffersAsNoTracking();
        IQueryable<Offer> GetOffers(Expression<Func<Offer, bool>> where, params Expression<Func<Offer, object>>[] includes);
        IQueryable<Offer> GetOffers(params Expression<Func<Offer, object>>[] includes);
        IQueryable<Offer> GetOffersAsNoTracking(Expression<Func<Offer, bool>> where, params Expression<Func<Offer, object>>[] includes);
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Offer GetOffer(int id);
        void Delete(Offer offer);
        Offer GetOffer(int id, params Expression<Func<Offer, object>>[] includes);
        //Task<Offer> GetOfferAsync(int id);
        //Task<IEnumerable<Offer>> GetOffersAsync(Expression<Func<Offer, bool>> where);
        decimal CalculateMiddlemanPrice(decimal offerPrice);
        IQueryable<Offer> SearchOffers(string game, string sort, ref bool isOnline, ref bool searchInDiscription,
            string searchString, ref int page, int pageSize,ref int totalItems, ref decimal minGamePrice, ref decimal maxGamePrice, ref decimal priceFrom, ref decimal priceTo, string[] filters);
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

        public IQueryable<Offer> GetOffers(params Expression<Func<Offer, object>>[] includes)
        {
            var offers = offersRepository.GetAll(includes);
            return offers;
        }

        public IQueryable<Offer> GetOffers(Expression<Func<Offer, bool>> where, params Expression<Func<Offer, object>>[] includes)
        {
            var query = offersRepository.GetMany(where, includes);
            return query;
        }
        public IQueryable<Offer> GetOffersAsNoTracking()
        {
            var query = offersRepository.GetAllAsNoTracking();
            return query;
        }
        public IQueryable<Offer> GetOffersAsNoTracking(Expression<Func<Offer, bool>> where, params Expression<Func<Offer, object>>[] includes)
        {
            var query = offersRepository.GetManyAsNoTracking(where, includes);
            return query;
        }



        //public async Task<IEnumerable<Offer>> GetOffersAsync(Expression<Func<Offer, bool>> where)
        //{
        //    var offers = await offersRepository.GetManyAsync(where);
        //    return offers;
        //}

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

        public Offer GetOffer(int id, params Expression<Func<Offer, object>>[] includes)
        {
            var offer = offersRepository.Get(o => o.Id == id, includes);
            return offer;
        }

        //public async Task<Offer> GetOfferAsync(int id)
        //{
        //    var offer = await offersRepository.GetAsync(o => o.Id == id);
        //    return offer;
        //}

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

        private IQueryable<Offer> SearchOffersByGame(string game)
        {
            return offersRepository.GetManyAsNoTracking(m => m.Game.Value == game, i => i.Game, i => i.UserProfile, i => i.FilterItems, i => i.Filters);
                        
        }

        private IQueryable<Offer> SearchOffersByFilters(IQueryable<Offer> offers, string[] filters)
        {
            
            IList<Offer> listOffers = new List<Offer>();
            if (filters != null)
            {
                if (offers.Any() && offers.FirstOrDefault().FilterItems.Count != filters.Length)
                {
                    listOffers = offers.ToList();
                }
                bool equals = false;
                foreach (var offer in offers.ToList())
                {
                    for (int i = 0; i < Math.Min(offer.FilterItems.Count, filters.Length); i++)
                    {

                        if (offer.FilterItems[i].Value != filters[i].Split('=')[1] && filters[i].Split('=')[1] != "empty")
                        {
                            if (offer.Filters[i].Value == filters[i].Split('=')[0])
                            {
                                equals = false;
                                break;
                            }


                        }

                        equals = true;
                    }
                    if (equals)
                    {
                        listOffers.Add(offer);
                    }
                    equals = false;

                }
            }
            else
            {
                return offers;
            }
            return listOffers.AsQueryable();
        }

        private IQueryable<Offer> SearchOffersByPrice(IQueryable<Offer> offers, ref decimal priceFrom, ref decimal priceTo, ref decimal minGamePrice,ref decimal maxGamePrice)
        {
            var offersList = offers;
            if (offers.Any())
            {
                minGamePrice = offersList.Min(m => m.Price);

                maxGamePrice = offersList.Max(m => m.Price);


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

                offersList = offersList.Where(offer => offer.Price >= priceF && offer.Price <= priceT);
            }
            
            return offersList;
        }

        private IQueryable<Offer> SearchOffersBySearchString(IQueryable<Offer> offers, string searchString, ref bool searchInDiscription)
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

        private IQueryable<Offer> SearchOffersByPage(IQueryable<Offer> offers,ref int page, int pageSize, ref int totalItems)

        {
            offers = offers.Skip((page - 1) * pageSize).Take(pageSize);
            return offers;
        }

        private IQueryable<Offer> SortOffers(IQueryable<Offer> offers, string sort)
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

        public IQueryable<Offer> SearchOffers(string game, string sort, ref bool isOnline, ref bool searchInDiscription,
            string searchString, ref int page,  int pageSize,ref int totalItems, ref decimal minGamePrice, ref decimal maxGamePrice, ref decimal priceFrom, ref decimal priceTo,string[] filters )
        {
            IQueryable<Offer> offers;
            offers = SearchOffersByGame(game);
            offers = SearchOffersByFilters(offers, filters);
            offers = offers.Where(o => o.State == OfferState.active);            
            offers = SearchOffersBySearchString(offers, searchString,ref searchInDiscription);
            offers = SearchOffersByPrice(offers, ref priceFrom, ref priceTo, ref minGamePrice, ref maxGamePrice);
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
