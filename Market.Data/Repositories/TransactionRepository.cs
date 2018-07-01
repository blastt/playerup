using Market.Data.Infrastructure;
using Market.Model.Models;

public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
{
    public TransactionRepository(IDbFactory dbFactory)
        : base(dbFactory) { }


}

public interface ITransactionRepository : IRepository<Transaction>
{

}
