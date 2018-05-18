using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }
}
