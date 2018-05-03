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
    public interface ISellerInvoiceService
    {
        IEnumerable<SellerInvoice> GetSellerInvoices();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        SellerInvoice GetSellerInvoice(int id);
        void UpdateSellerInvoice(SellerInvoice sellerInvoice);
        void CreateSellerInvoice(SellerInvoice sellerInvoice);
        void SaveSellerInvoice();
    }

    public class SellerInvoiceService : ISellerInvoiceService
    {
        private readonly ISellerInvoiceRepository sellerInvoicesRepository;
        private readonly IUnitOfWork unitOfWork;

        public SellerInvoiceService(ISellerInvoiceRepository sellerInvoicesRepository, IUnitOfWork unitOfWork)
        {
            this.sellerInvoicesRepository = sellerInvoicesRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ISellerInvoiceService Members

        public IEnumerable<SellerInvoice> GetSellerInvoices()
        {
            var sellerInvoices = sellerInvoicesRepository.GetAll();
            return sellerInvoices;
        }

        public void UpdateSellerInvoice(SellerInvoice sellerInvoice)
        {
            sellerInvoicesRepository.Update(sellerInvoice);
        }
        public SellerInvoice GetSellerInvoice(int id)
        {
            var sellerInvoice = sellerInvoicesRepository.GetById(id);
            return sellerInvoice;
        }



        public void CreateSellerInvoice(SellerInvoice sellerInvoice)
        {
            sellerInvoicesRepository.Add(sellerInvoice);
        }

        public void SaveSellerInvoice()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
