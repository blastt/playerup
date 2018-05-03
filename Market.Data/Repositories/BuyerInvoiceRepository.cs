using Market.Data.Infrastructure;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Repositories
{
    public class BuyerInvoiceRepository : RepositoryBase<BuyerInvoice>, IBuyerInvoiceRepository
    {
        public BuyerInvoiceRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }

    public interface IBuyerInvoiceRepository : IRepository<BuyerInvoice>
    {

    }
}
