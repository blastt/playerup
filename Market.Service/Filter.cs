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
    
    public interface IFilterService
    {
        IEnumerable<Filter> GetFilters();
        Filter GetFilter(int id);
        void Delete(Filter filter);
        void CreateFilter(Filter message);
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
