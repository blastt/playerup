using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Market.Service
{
    public interface IWithdrawService
    {
        IEnumerable<Withdraw> GetWithdraws();
        IQueryable<Withdraw> GetWithdrawsAsNoTracking();
        IQueryable<Withdraw> GetWithdraws(Expression<Func<Withdraw, bool>> where, params Expression<Func<Withdraw, object>>[] includes);
        IQueryable<Withdraw> GetWithdrawsAsNoTracking(params Expression<Func<Withdraw, object>>[] includes);
        IQueryable<Withdraw> GetWithdrawsAsNoTracking(Expression<Func<Withdraw, bool>> where, params Expression<Func<Withdraw, object>>[] includes);

        Withdraw GetWithdraw(int id);
        void Delete(Withdraw Withdraw);

        void CreateWithdraw(Withdraw message);
        void SaveWithdraw();
    }

    public class WithdrawService : IWithdrawService
    {
        private readonly IWithdrawRepository WithdrawsRepository;
        private readonly IUnitOfWork unitOfWork;

        public WithdrawService(IWithdrawRepository WithdrawsRepository, IUnitOfWork unitOfWork)
        {
            this.WithdrawsRepository = WithdrawsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IWithdrawService Members

        public IEnumerable<Withdraw> GetWithdraws()
        {
            var withdraws = WithdrawsRepository.GetAll();
            return withdraws;
        }

        

        public IQueryable<Withdraw> GetWithdrawsAsNoTracking()
        {
            var query = WithdrawsRepository.GetAllAsNoTracking();
            return query;
        }

        public IQueryable<Withdraw> GetWithdrawsAsNoTracking(params Expression<Func<Withdraw, object>>[] includes)
        {
            var withdraws = WithdrawsRepository.GetAllAsNoTracking(includes);
            return withdraws;
        }

        public IQueryable<Withdraw> GetWithdraws(Expression<Func<Withdraw, bool>> where, params Expression<Func<Withdraw, object>>[] includes)
        {
            var query = WithdrawsRepository.GetMany(where, includes);
            return query;
        }

        public IQueryable<Withdraw> GetWithdrawsAsNoTracking(Expression<Func<Withdraw, bool>> where, params Expression<Func<Withdraw, object>>[] includes)
        {
            var query = WithdrawsRepository.GetManyAsNoTracking(where, includes);
            return query;
        }


        public Withdraw GetWithdraw(int id)
        {
            var Withdraw = WithdrawsRepository.GetById(id);
            return Withdraw;
        }


        public void CreateWithdraw(Withdraw Withdraw)
        {
            WithdrawsRepository.Add(Withdraw);
        }

        public void Delete(Withdraw Withdraw)
        {
            WithdrawsRepository.Delete(Withdraw);
        }

        public void SaveWithdraw()
        {
            unitOfWork.Commit();
        }


        #endregion

    }
}
