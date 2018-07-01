using Market.Data.Infrastructure;

namespace Marketplace.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        MarketEntities dbContext;

        public MarketEntities Init()
        {
            return dbContext ?? (dbContext = new MarketEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
