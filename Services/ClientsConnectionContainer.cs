using PackageCopycat.Interfaces;
using PackageCopycat.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Services
{
    public class ClientsConnectionContainer : IClientsContainer
    {
        private readonly ConcurrentDictionary<string, RabbitConsumer> _consumers;

        public ClientsConnectionContainer()
        {
            _consumers = new ConcurrentDictionary<string, RabbitConsumer>();
        }

        public RabbitConsumer GetConsumer(string clientId)
        {

            if (_consumers.TryGetValue(clientId, out RabbitConsumer consumer))
            {
                return consumer;
            }
            return null;
        }
        public bool AddConnection(string clientId, RabbitConsumer consumer)
        {
            _consumers[clientId] = consumer;
            return true ;
        }

        public bool TryRemove(string clientId)
        {
            return _consumers.TryRemove(clientId, out _);
        }
    }
}
