using PackageCopycat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IClientsContainer
    {
        bool AddConnection(string clientId, RabbitConsumer consumer);
        bool TryRemove(string clientId);
        RabbitConsumer GetConsumer(string clientId);
    }
}
