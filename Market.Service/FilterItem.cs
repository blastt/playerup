using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System.Collections.Generic;

namespace Market.Service
{
    public interface IFilterItemService
    {
        IEnumerable<FilterItem> GetFilterItems();
        FilterItem GetFilterItem(int id);
        void CreateFilterItem(FilterItem message);
        void SaveFilterItem();
    }

    public class FilterItemService : IFilterItemService
    {
        private readonly IFilterItemRepository filterItemsRepository;
        private readonly IUnitOfWork unitOfWork;

        public FilterItemService(IFilterItemRepository filterItemsRepository, IUnitOfWork unitOfWork)
        {
            this.filterItemsRepository = filterItemsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IFilterService Members

        public IEnumerable<FilterItem> GetFilterItems()
        {
            var filterItems = filterItemsRepository.GetAll();
            return filterItems;
        }


        public FilterItem GetFilterItem(int id)
        {
            var filterItem = filterItemsRepository.GetById(id);
            return filterItem;
        }


        public void CreateFilterItem(FilterItem filterItem)
        {
            filterItemsRepository.Add(filterItem);
        }

        public void SaveFilterItem()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
