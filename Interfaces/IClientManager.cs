using PackageCopycat.Models;
using System;
using System.Threading.Tasks;

namespace PackageCopycat.Services
{
    public interface IClientManager
    {
        Task Start(QueueMessagesRequest request);
    }
}