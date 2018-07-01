using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


    }

    public interface IMessageRepository : IRepository<Message>
    {

    }
}
