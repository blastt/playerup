using Autofac;
using Autofac.Integration.Mvc;
using Hangfire;
using Market.Data.Infrastructure;
using Market.Data.Repositories;
using Market.Service;
using Market.Web.Hangfire;
using Market.Web.Mappings;
using Marketplace.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Market.Web.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainer();
            AutoMapperConfiguration.Configure();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<EmailService>().As<IIdentityMessageService>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>(). InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<CultureInfo>().As<IFormatProvider>().InstancePerRequest().WithParameter("name","en-US");
            
            #region Repositories

            builder.RegisterAssemblyTypes(typeof(OfferRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(OrderRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(MessageRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FeedbackRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(UserProfileRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(GameRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FilterRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FilterItemRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            #endregion

            #region Services

            builder.RegisterAssemblyTypes(typeof(OfferService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(OrderService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(MessageService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FeedbackService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(UserProfileService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(GameService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FilterService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(FilterItemService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            #endregion

            //builder.RegisterType<MyJob>().AsSelf();
            //builder.RegisterType<OrderService>().As<IOrderService>();
            IContainer container = builder.Build();
            //GlobalConfiguration.Configuration.UseActivator(new MarketJobActivator(container));
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
