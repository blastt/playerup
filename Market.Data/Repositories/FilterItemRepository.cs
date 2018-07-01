using Market.Data.Infrastructure;
using Market.Model.Models;

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
