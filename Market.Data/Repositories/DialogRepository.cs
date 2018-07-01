using Market.Data.Infrastructure;
using Market.Model.Models;

namespace Market.Data.Repositories
{
    public class DialogRepository : RepositoryBase<Dialog>, IDialogRepository
    {
        public DialogRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


    }

    public interface IDialogRepository : IRepository<Dialog>
    {

    }
}
