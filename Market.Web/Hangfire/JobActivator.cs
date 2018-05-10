using Autofac;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Hangfire
{
    public class MarketJobActivator : JobActivator
    {
        private IContainer _container;

        public MarketJobActivator(IContainer container)
        {
            _container = container;

        }

        public override object ActivateJob(Type type)
        {
            return _container.Resolve(type);
        }
    }
}