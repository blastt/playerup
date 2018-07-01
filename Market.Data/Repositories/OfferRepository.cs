using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class OfferRepository : RepositoryBase<Offer>, IOfferRepository
    {
        public OfferRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        
    }

    public interface IOfferRepository : IRepository<Offer>
    {
        
    }
}
