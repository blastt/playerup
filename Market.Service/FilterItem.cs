﻿using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Market.Service
{
    public interface IFilterItemService
    {
        IEnumerable<FilterItem> GetFilterItems();
        FilterItem GetFilterItem(int id);
        IQueryable<FilterItem> GetFilterItemsAsNoTracking(params Expression<Func<FilterItem, object>>[] includes);
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

        public IQueryable<FilterItem> GetFilterItemsAsNoTracking(params Expression<Func<FilterItem, object>>[] includes)
        {
            var query = filterItemsRepository.GetAll(includes);
            return query;
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
