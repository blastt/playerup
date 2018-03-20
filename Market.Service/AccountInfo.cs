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
    public interface IAccountInfoService
    {
        IEnumerable<AccountInfo> GetAccountInfos();
        AccountInfo GetAccountInfo(int id);
        void DeleteAccountInfo(AccountInfo feedback);
        void UpdateAccountInfo(AccountInfo feedback);
        void CreateAccountInfo(AccountInfo feedback);
        void SaveAccountInfo();
    }

    public class AccountInfoService : IAccountInfoService
    {
        private readonly IAccountInfoRepository accountInfoRepository;
        private readonly IUnitOfWork unitOfWork;

        public AccountInfoService(IAccountInfoRepository accountInfoRepository, IUnitOfWork unitOfWork)
        {
            this.accountInfoRepository = accountInfoRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IMessageService Members

        public IEnumerable<AccountInfo> GetAccountInfos()
        {
            var feedback = accountInfoRepository.GetAll();
            return feedback;
        }


        public AccountInfo GetAccountInfo(int id)
        {
            var feedback = accountInfoRepository.GetById(id);
            return feedback;
        }


        public void CreateAccountInfo(AccountInfo feedback)
        {
            accountInfoRepository.Add(feedback);
        }

        public void SaveAccountInfo()
        {
            unitOfWork.Commit();
        }

        public void DeleteAccountInfo(AccountInfo feedback)
        {
            accountInfoRepository.Delete(feedback);
        }

        public void UpdateAccountInfo(AccountInfo feedback)
        {
            accountInfoRepository.Update(feedback);
        }

        #endregion

    }
}
