using Autofac;
using Market.Data.Repositories;
using Market.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Market.Windows_Service
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        /// 

        static void Main()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(OrderRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterAssemblyTypes(typeof(OrderService).Assembly)
              .Where(t => t.Name.EndsWith("Service"))
              .AsImplementedInterfaces().InstancePerRequest();
            var container = builder.Build();
            var orderService = container.Resolve<IOrderService>();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service2(orderService)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
