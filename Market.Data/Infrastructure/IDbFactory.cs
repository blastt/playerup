using Marketplace.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        MarketEntities Init();
    }
}
