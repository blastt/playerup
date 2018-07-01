using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


    }

    public interface IFeedbackRepository : IRepository<Feedback>
    {

    }
}
