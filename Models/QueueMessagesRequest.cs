using PackageCopycat.Abstracts.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Models
{
    public class QueueMessagesRequest : RequestBase
    {
        public string QueueName { get; set; }
        public string ConnectionId { get; set; }
    }
}
