using PackageCopycat.Models;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IPublisher
    {
        Task Send(QueueMessagesRequest request, BasicDeliverEventArgs queueMessage);
        Task<bool> SaveGroup(QueueMessagesRequest request);
    }
}
