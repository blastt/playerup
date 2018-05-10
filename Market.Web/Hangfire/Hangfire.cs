using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Market.Web.Hangfire;
using Autofac;

using Market.Service;
using Autofac.Integration.Mvc;
using Market.Data.Repositories;
using System.Globalization;
using Market.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Reflection;
using Marketplace.Data.Infrastructure;
using System.Web.Mvc;

namespace Market.Web.Hangfire
{
    public class MyHangfire
    {
        public static void ConfigureHangfire(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<EmailService>().As<IIdentityMessageService>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<CultureInfo>().As<IFormatProvider>().WithParameter("name", "en-US").InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(OrderRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(OrderService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<OrderCloseJob>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<OrderService>().As<IOrderService>();
            IContainer container = builder.Build();
            GlobalConfiguration.Configuration.UseActivator(new MarketJobActivator(container));

        }

        public static void InitializeJobs()
        {
            //BackgroundJob.Schedule<OrderCloseJob>(
            //    j => j.Do(),
            //    TimeSpan.FromMinutes(1));
            //RecurringJob.AddOrUpdate<MyJob>(j => j.Execute(), "* * * * *");
        }
    }
}