using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Market.Service
{
    
    public interface IFilterService
    {
        IEnumerable<Filter> GetFilters();
        IQueryable<Filter> GetFilters(Expression<Func<Filter, bool>> where, params Expression<Func<Filter, object>>[] includes);
        IQueryable<Filter> GetFiltersAsNoTracking(Expression<Func<Filter, bool>> where, params Expression<Func<Filter, object>>[] includes);
        IQueryable<Filter> GetFiltersAsNoTracking(params Expression<Func<Filter, object>>[] includes);
        Filter GetFilter(int id);
        void Delete(Filter filter);
        void CreateFilter(Filter message);
        Filter GetFilterByValue(string value);
        void SaveFilter();
    }

    public class FilterService : IFilterService
    {
        private readonly IFilterRepository filtersRepository;
        private readonly IUnitOfWork unitOfWork;

        public FilterService(IFilterRepository filtersRepository, IUnitOfWork unitOfWork)
        {
            this.filtersRepository = filtersRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IFilterService Members

        public IEnumerable<Filter> GetFilters()
        {
            var filters = filtersRepository.GetAll();
            return filters;
        }

        public IQueryable<Filter> GetFilters(Expression<Func<Filter, bool>> where, params Expression<Func<Filter, object>>[] includes)
        {
            var query = filtersRepository.GetMany(where, includes);
            return query;
        }

        public IQueryable<Filter> GetFiltersAsNoTracking(params Expression<Func<Filter, object>>[] includes)
        {
            var query = filtersRepository.GetAll(includes);
            return query;
        }

        public IQueryable<Filter> GetFiltersAsNoTracking(Expression<Func<Filter, bool>> where, params Expression<Func<Filter, object>>[] includes)
        {
            var query = filtersRepository.GetManyAsNoTracking(where);
            return query;
        }

     
        public Filter GetFilterByValue(string value)
        {
            return filtersRepository.GetFilterByValue(value);
        }

        public Filter GetFilter(int id)
        {
            var filter = filtersRepository.GetById(id);
            return filter;
        }
        public void Delete(Filter filter)
        {
            filtersRepository.Delete(filter);
        }

        public void CreateFilter(Filter filter)
        {
            filtersRepository.Add(filter);
        }

        public void SaveFilter()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
    
}
