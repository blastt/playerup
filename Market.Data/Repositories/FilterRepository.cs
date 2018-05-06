using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Repositories
{
    public class FilterRepository : RepositoryBase<Filter>, IFilterRepository
    {
        public FilterRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Filter GetFilterByValue(string value)
        {
            return DbContext.Filters.FirstOrDefault(f => f.Value == value);
        }
    }

    public interface IFilterRepository : IRepository<Filter>
    {
        Filter GetFilterByValue(string value);
    }
}
