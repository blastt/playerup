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
using Market.Model.Models;

namespace Market.Web.Hangfire
{
    public class MarketHangfire
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

        public static string SetOrderCloseJob(Order order, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<OrderCloseJob>(
                j => j.Do(order),
                timeSpan);
        }
        public static string SetConfirmOrderJob(int orderId, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<ConfirmOrderJob>(
                j => j.Do(orderId),
                timeSpan);
        }
        public static string SetLeaveFeedbackJob(int orderId, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<LeaveFeedbackJob>(
                j => j.Do(orderId),
                timeSpan);
        }
        public static string SetDeactivateOfferJob(int offerId, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<DeactivateOfferJob>(
                j => j.Do(offerId),
                timeSpan);
        }
    }
}