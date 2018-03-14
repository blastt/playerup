using Autofac;
using Market.Service;
using Market.Web.SR.Mappings;
using Market.Web.SR.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Market.Web.SR.Hubs
{
    public class StatusHub : Hub
    {
        static ConnectionMapping _connections = new ConnectionMapping();

        private void IsActive(string connection, bool connected)
        {
            Clients.All.clientconnected(connection, connected);
        }
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            _connections.Add(name, Context.ConnectionId);
            IsActive(name, true);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);
            IsActive(name, false);
            return base.OnDisconnected(stopCalled);
        }
        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;
        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }
        //    IsActive(name, false);
        //    return base.OnReconnected();
        //}
    }
}