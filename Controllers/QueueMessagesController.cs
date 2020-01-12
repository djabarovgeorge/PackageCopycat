using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PackageCopycat.Abstracts.Bases;
using PackageCopycat.Interfaces;
using PackageCopycat.Models;
using PackageCopycat.Services;

namespace PackageCopycat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueMessagesController : ControllerBase
    {
        private readonly IFactory _factory;
        public QueueMessagesController(IFactory factory)
        {
            _factory = factory;
        }
        [HttpPost]
        public async Task<ResponseBase> PostQueueMessages(QueueMessagesRequest request)
        {
            return await _factory.GetHandler(request.GetType()).HandleAsync(request);
        }
    }
}