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
    public interface IBuyerInvoiceService
    {
        IEnumerable<BuyerInvoice> GetBuyerInvoices();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        BuyerInvoice GetBuyerInvoice(int id);
        void UpdateBuyerInvoice(BuyerInvoice buyerInvoice);
        void CreateBuyerInvoice(BuyerInvoice buyerInvoice);
        void SaveBuyerInvoice();
    }

    public class BuyerInvoiceService : IBuyerInvoiceService
    {
        private readonly IBuyerInvoiceRepository buyerInvoicesRepository;
        private readonly IUnitOfWork unitOfWork;

        public BuyerInvoiceService(IBuyerInvoiceRepository buyerInvoicesRepository, IUnitOfWork unitOfWork)
        {
            this.buyerInvoicesRepository = buyerInvoicesRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IBuyerInvoiceService Members

        public IEnumerable<BuyerInvoice> GetBuyerInvoices()
        {
            var buyerInvoices = buyerInvoicesRepository.GetAll();
            return buyerInvoices;
        }

        public void UpdateBuyerInvoice(BuyerInvoice buyerInvoice)
        {
            buyerInvoicesRepository.Update(buyerInvoice);
        }
        public BuyerInvoice GetBuyerInvoice(int id)
        {
            var buyerInvoice = buyerInvoicesRepository.GetById(id);
            return buyerInvoice;
        }



        public void CreateBuyerInvoice(BuyerInvoice buyerInvoice)
        {
            buyerInvoicesRepository.Add(buyerInvoice);
        }

        public void SaveBuyerInvoice()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
