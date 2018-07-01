using Hangfire;
using Owin;
using System;
using Autofac;

using Market.Service;
using Autofac.Integration.Mvc;
using Market.Data.Repositories;
using Market.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using System.Reflection;
using Marketplace.Data.Infrastructure;

namespace Market.Web.Hangfire
{
    public class MarketHangfire
    {
        public static void ConfigureHangfire(IAppBuilder app)
        {
            

            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //builder.RegisterType<EmailService>().As<IIdentityMessageService>();
            builder.RegisterType<EmailService>().As<IIdentityMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerLifetimeScope();
            //builder.RegisterType<CultureInfo>().As<IFormatProvider>().WithParameter("name", "en-US").InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(OrderRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(FeedbackRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(OrderService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(FeedbackService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<OrderCloseJob>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ConfirmOrderJob>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<LeaveFeedbackJob>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<DeactivateOfferJob>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SendEmailChangeStatus>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<OrderService>().As<IOrderService>();
            IContainer container = builder.Build();
            GlobalConfiguration.Configuration.UseActivator(new MarketJobActivator(container));

        }

        public static string SetOrderCloseJob(int orderId, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<OrderCloseJob>(
                j => j.Do(orderId),
                timeSpan);
        }
        public static string SetConfirmOrderJob(int orderId, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<ConfirmOrderJob>(
                j => j.Do(orderId),
                timeSpan);
        }
        public static string SetLeaveFeedbackJob(string sellerId, string buyerId, int orderId, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<LeaveFeedbackJob>(
                j => j.Do(sellerId, buyerId, orderId),
                timeSpan);
        }
        public static string SetDeactivateOfferJob(int offerId,string callbackUrl, TimeSpan timeSpan)
        {
            return BackgroundJob.Schedule<DeactivateOfferJob>(
                j => j.Do(offerId, callbackUrl),
                timeSpan);
        }

        public static string SetSendEmailChangeStatus(int orderId, string userEmail , string currentStatus, string callbackUrl)
        {
            return BackgroundJob.Enqueue<SendEmailChangeStatus>(
                j => j.Do(orderId, userEmail, currentStatus, callbackUrl));
        }
    }
}