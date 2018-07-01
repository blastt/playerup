using Marketplace.Data;
using System;

namespace Market.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        MarketEntities Init();
    }
}
