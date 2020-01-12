using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PackageCopycat.Abstracts;
using PackageCopycat.Configs;
using PackageCopycat.Interfaces;
using PackageCopycat.Models;
using PackageCopycat.Models.DBModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PackageCopycat.Services
{
    public class ListenerService : IListenerService, IDisposable
    {
        public event EventHandler<ReceivedMessageEvent> MessageRecived;

        private IConnectionFactory _connectionFactory;
        private RabbitApiBindingConfigs _bindingApiConfigs;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;
        private bool _disposed;

        public ListenerService(IConnectionFactory connectionFactory, RabbitApiBindingConfigs bindingApiConfigs)
        {
            _connectionFactory = connectionFactory;
            _bindingApiConfigs = bindingApiConfigs;
            _disposed = false;
        }
        public async Task<RabbitConsumer> InitConsumer(string queueName)
        {
            RabbitConsumer consumer = null;
            _queueName = queueName;
            if (!await EnsureConnection())
            {
                throw new Exception("Failed asablishing connection with RabbitMQ");
            }

            try
            {
                consumer = await Consume(_queueName);
            }
            catch (Exception)
            {
                throw;
            }
            return consumer;
        }

        public async Task<RabbitConsumer> Consume(string queueId)
        {
            if (_connectionFactory == null)
                throw new Exception("Connection is not configured");

            if (!await EnsureConnection())
                throw new Exception("Could not astablish connection to rabbit");

            return await SetupChannel(queueId);
        }
 

        #region private helpers

        private bool Connected => _connection != null && _connection.IsOpen;

        private async Task<bool> EnsureConnection(bool reconnectIfFailed = true)
        {
            try
            {
                if (!Connected)
                {
                    if (_connection != null)
                    {
                        // Dispose of dead hanging connection
                        _connection.Dispose();
                    }
                    try
                    {
                        _connection = _connectionFactory.CreateConnection();
                    }
                    catch (BrokerUnreachableException brockerUnreachedException)
                    {
                        if (reconnectIfFailed)
                        {
                            Thread.Sleep(2000);
                            return await EnsureConnection(false);
                        }
                        //TODO Ask what happent after two failed tries to connect
                    }

                    if (Connected)
                    {
                        // Connection error handlers
                        _connection.ConnectionShutdown += async (object sender, ShutdownEventArgs reason) => await OnConnectionShutdown(sender, reason);
                        _connection.CallbackException += async (object sender, CallbackExceptionEventArgs ex) => await OnCallbackException(sender, ex);
                        _connection.ConnectionBlocked += async (object sender, ConnectionBlockedEventArgs ex) => await OnConnectionBlocked(sender, ex);

                        // Log"RabbitMQ client had been connected successfully"
                    }
                    else
                    {
                        // Log"RabbitMQ client had FAILED to connect"
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Connected;
        }

        private async Task OnConnectionBlocked(object sender, ConnectionBlockedEventArgs ex)
        {
            await HandleConnectionProblem("A RabbitMQ connection is shutdown. Trying to re-connect...", nameof(OnConnectionShutdown));
        }

        private async Task OnCallbackException(object sender, CallbackExceptionEventArgs ex)
        {
            await HandleConnectionProblem("A RabbitMQ connection throw exception. Trying to re-connect...", nameof(OnCallbackException));
        }

        private async Task OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            await HandleConnectionProblem("A RabbitMQ connection is on shutdown. Trying to re-connect...", nameof(OnConnectionShutdown));
        }

        private async Task HandleConnectionProblem(string message, string functionName)
        {
            if (_disposed)
            {
                return;
            }
            await EnsureConnection();
        }

        private async Task<QueueBinding> GetBinding(string queueName)
        {
            List<QueueBinding> responseObj = null;
            try
            {
                var uriResult = $"{_bindingApiConfigs.ApiBindingPrefixRequest}{queueName}{_bindingApiConfigs.ApiBindingSufixRequest}";
                using (var clientNew = new HttpClient())
                {
                    var byteArray = Encoding.ASCII.GetBytes($"{_connectionFactory.UserName}:{_connectionFactory.Password}");
                    clientNew.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage responseNew = await clientNew.GetAsync(uriResult);
                    HttpContent content = responseNew.Content;

                    string result = await content.ReadAsStringAsync();

                    responseObj = JsonConvert.DeserializeObject<List<QueueBinding>>(result);
                };
            }
            catch (Exception ex)
            {
                throw;
            }
            return responseObj[1];
        }

        #endregion
        public async Task<RabbitConsumer> SetupChannel(string queue, bool retryOnFailure = true)
        {
            RabbitConsumer consumer = null;
            if (await EnsureConnection())
            {
                try
                {
                    _channel = _connection.CreateModel();

                    consumer = new RabbitConsumer(_channel, _queueName);

                    var queueBinding = await GetBinding(queue);

                    _channel.ExchangeDeclare(queueBinding.source, ExchangeType.Topic);

                    _channel.QueueDeclare(queue: $"Mirror{queue}", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    _channel.QueueBind(queue: $"Mirror{queue}",
                                        exchange: queueBinding.source,
                                        routingKey: queueBinding.routing_key);

                    _channel.BasicQos(0, 1000, global: false);

                    _channel.BasicConsume(queue: $"Mirror{queue}", autoAck: true, consumer: consumer);

                    _channel.CallbackException += async (sender, ea) =>
                    {
                        _channel.Dispose();

                        if (retryOnFailure)
                        {
                            await SetupChannel(queue, false);
                        }
                    };
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return consumer;
        }
        public void Dispose()
        {
            if (_connection != null && _disposed)
            {
                return;
            }
            try
            {
                _disposed = true;
                _connection.Dispose();

            }
            catch (Exception ex)
            {

            }
        }
    
    }
}