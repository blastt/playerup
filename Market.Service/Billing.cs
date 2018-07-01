using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System.Collections.Generic;

namespace Market.Service
{
    public interface IBillingService
    {
        IEnumerable<Billing> GetBillings();
        //IEnumerable<Offer> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Billing GetBilling(int id);
        void UpdateBilling(Billing billing);
        void CreateBilling(Billing billing);
        void SaveBilling();
    }

    public class BillingService : IBillingService
    {
        private readonly IBillingRepository billingsRepository;
        private readonly IUnitOfWork unitOfWork;

        public BillingService(IBillingRepository billingsRepository, IUnitOfWork unitOfWork)
        {
            this.billingsRepository = billingsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IBillingService Members

        public IEnumerable<Billing> GetBillings()
        {
            var billings = billingsRepository.GetAll();
            return billings;
        }

        public void UpdateBilling(Billing billing)
        {
            billingsRepository.Update(billing);
        }
        public Billing GetBilling(int id)
        {
            var billing = billingsRepository.GetById(id);
            return billing;
        }



        public void CreateBilling(Billing billing)
        {
            billingsRepository.Add(billing);
        }

        public void SaveBilling()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
