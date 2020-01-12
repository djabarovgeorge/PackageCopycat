using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using PackageCopycat.Interfaces;
using PackageCopycat.Models;


namespace PackageCopycat.Hubs
{
    public class PackagesHub : Hub<IPackagesHub>
    {
        private readonly IClientsContainer _container;
        public PackagesHub(IClientsContainer container)
        {
            _container = container;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_container.GetConsumer(Context.ConnectionId) != null)
            {
                _container.GetConsumer(Context.ConnectionId).Dispose();
                _container.TryRemove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
