using System.Threading.Tasks;

namespace Market.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        Task CommitAsync();
    }
}
