using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
