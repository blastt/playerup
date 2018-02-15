using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Repositories
{
    public class FilterItemRepository : RepositoryBase<FilterItem>, IFilterItemRepository
    {
        public FilterItemRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


    }

    public interface IFilterItemRepository : IRepository<FilterItem>
    {

    }
}
