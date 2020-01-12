using Microsoft.AspNetCore.SignalR;
using PackageCopycat.Hubs;
using PackageCopycat.Interfaces;
using PackageCopycat.Models;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageCopycat.Services
{
    public class HubPublisher : IPublisher
    {
        private IHubContext<PackagesHub, IPackagesHub> _hubContext;
        public HubPublisher(IHubContext<PackagesHub, IPackagesHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task Send(QueueMessagesRequest request, BasicDeliverEventArgs queueMessage)
        {

            await _hubContext.Groups.AddToGroupAsync(request.ConnectionId, request.QueueName);

            var responseBody = queueMessage.Body;
            var responseString = Encoding.UTF8.GetString(responseBody);

            if(responseString != null)
            {
                try
                {
                    await _hubContext.Clients.Group(request.QueueName).MessageReceived(responseString);

                }
                catch (Exception ex)
                {
                    await _hubContext.Clients.Group(request.QueueName).Excepion(ex.ToString());
                }
            }
        }
        public async Task<bool> SaveGroup(QueueMessagesRequest request)
        {
            bool ifExist = false; 
            if (_hubContext.Clients.Group(request.QueueName) != null)
            {
                ifExist = true;
            }
            await _hubContext.Groups.AddToGroupAsync(request.ConnectionId, request.QueueName);

            return ifExist;

        }
    }
}
