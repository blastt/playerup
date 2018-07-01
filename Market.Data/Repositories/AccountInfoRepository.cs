using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class AccountInfoRepository : RepositoryBase<AccountInfo>, IAccountInfoRepository
    {
        public AccountInfoRepository(IDbFactory dbFactory)
            : base(dbFactory) { }



        
    }

    public interface IAccountInfoRepository : IRepository<AccountInfo>
    {
    }
}
