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

    public interface ITransactionService
    {
        IEnumerable<Transaction> GetTransactions();
        //IEnumerable<Transaction> GetCategoryGadgets(string categoryName, string gadgetName = null);
        Transaction GetTransaction(int id);
        void UpdateTransaction(Transaction transaction);
        void CreateTransaction(Transaction transaction);
        void SaveTransaction();
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository transactionsRepository;
        private readonly IFeedbackRepository feedbacksRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserProfileRepository userProfileRepository;

        public TransactionService(ITransactionRepository transactionsRepository, IFeedbackRepository feedbacksRepository,  IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            this.feedbacksRepository = feedbacksRepository;
            this.transactionsRepository = transactionsRepository;
            this.unitOfWork = unitOfWork;
            this.userProfileRepository = userProfileRepository;
        }

        #region ITransactionService Members

        public IEnumerable<Transaction> GetTransactions()
        {
            var transactions = transactionsRepository.GetAll();
            return transactions;
        }

        public void UpdateTransaction(Transaction transaction)
        {
            transactionsRepository.Update(transaction);
        }
        public Transaction GetTransaction(int id)
        {
            var transaction = transactionsRepository.GetById(id);
            return transaction;
        }


        public void CreateTransaction(Transaction transaction)
        {
            transactionsRepository.Add(transaction);
        }

        public void SaveTransaction()
        {
            unitOfWork.Commit();
        }

       

        #endregion

    }
}
