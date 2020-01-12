using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PackageCopycat.Abstracts;
using PackageCopycat.Interfaces;
using PackageCopycat.Models;

namespace PackageCopycat.Services
{
    public class ClientManager : IClientManager
    {
        private readonly IListenerService _listenerService;
        private readonly IPublisher _publisher;
        private readonly IClientsContainer _clientsContainer;

        public ClientManager(IListenerService listenerService, IPublisher publisher, IClientsContainer clientsContainer)
        {
            _listenerService = listenerService;
            _publisher = publisher;
            _clientsContainer = clientsContainer;
        }
        public async Task Start(QueueMessagesRequest request)
        {
            try
            {
                var consumer = await _listenerService.InitConsumer(request.QueueName);

                _clientsContainer.AddConnection(request.ConnectionId, consumer);

                consumer.Received += async (source, e) =>
                {
                    await _publisher.Send(request, e);
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
