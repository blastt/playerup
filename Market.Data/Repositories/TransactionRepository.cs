using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
{
    public TransactionRepository(IDbFactory dbFactory)
        : base(dbFactory) { }


}

public interface ITransactionRepository : IRepository<Transaction>
{

}
