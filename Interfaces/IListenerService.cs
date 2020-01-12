using PackageCopycat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IListenerService : IDisposable
    {
        public event EventHandler<ReceivedMessageEvent> MessageRecived;
        Task<RabbitConsumer> Consume(string queueId);
        Task<RabbitConsumer> InitConsumer(string queueName);

    }
}
