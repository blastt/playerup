using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service
{
    public interface IOfferService
    {
        IEnumerable<Offer> GetOffers();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Offer GetOffer(int id);
        void CreateOffer(Offer offer);
        void SaveOffer();
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
            var gadgets = offersRepository.GetAll();
            return gadgets;
        }


        public Offer GetOffer(int id)
        {
            var gadget = offersRepository.GetById(id);
            return gadget;
        }

        public void CreateOffer(Offer offer)
        {
            offersRepository.Add(offer);
        }

        public void SaveOffer()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
