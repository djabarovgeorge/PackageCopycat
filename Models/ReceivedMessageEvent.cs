using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Models
{
    public class ReceivedMessageEvent : BasicDeliverEventArgs
    {
        public bool Acknowledge { get; set; }
    }
}