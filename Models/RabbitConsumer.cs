using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PackageCopycat.Configs;
using PackageCopycat.Interfaces;
using PackageCopycat.Models.DBModels;
using PackageCopycat.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PackageCopycat.Models
{
    public sealed class RabbitConsumer : EventingBasicConsumer, IDisposable
    {
        private bool _disposed = false;
        private string _queueName;
        public RabbitConsumer(IModel channel, string queueName) : base(channel) { _queueName = queueName;}

        public void Dispose()
        {
            if (Model != null && _disposed)
            {
                return;
            }
            try
            {
                _disposed = true;
                Model.BasicCancel(ConsumerTag);
                Model.QueueDelete($"Mirror{_queueName}", true, false);
                Model.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        private void Raise<TEvent>(EventHandler<TEvent> eventHandler, TEvent evt)
        {
            var handler = eventHandler;
            if (handler != null)
            {
                handler(this, evt);
            }
        }
    }
}
