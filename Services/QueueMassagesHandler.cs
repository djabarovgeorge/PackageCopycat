using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using PackageCopycat.Abstracts;
using PackageCopycat.Abstracts.Bases;
using PackageCopycat.Configs;
using PackageCopycat.Controllers;
using PackageCopycat.Hubs;
using PackageCopycat.Interfaces;
using PackageCopycat.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageCopycat.Services
{
    public class QueueMassagesHandler : HandlerBase<QueueMessagesRequest>
    {
        public override Type Key => typeof(QueueMessagesRequest);

        private IClientManager _clientManager;
        public QueueMassagesHandler(IClientManager clientManager)
        {
            _clientManager = clientManager;
        }
        protected override async Task<ResponseBase> HandleInternalAsync(QueueMessagesRequest request)
        {
            ResponseBase response = null;
            Stopwatch watch = new Stopwatch();
            var executionTime = watch.ElapsedMilliseconds;

            try
            {
                watch.Start();
                await _clientManager.Start(request);
                watch.Stop();

                response = new QueueMessagesResponse("200", "Successfully connected to rabbitmq", $"Execution Time in MS: { executionTime }");
            }
            catch (Exception)
            {

                response = new QueueMessagesResponse("500", $"Failed to connected to rabbitmq", $"Execution Time in MS: { executionTime }");
            }
            return response;
        }
    }
}
