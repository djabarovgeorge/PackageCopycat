using Newtonsoft.Json;
using PackageCopycat.Abstracts.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Models
{
    public class QueueMessagesResponse : ResponseBase
    {
        public override string Status { get; set; }
        public override string Message { get; set; }
        public string ExecutionTime { get; set; }
        public bool SuccessfulResponse => Status == "200";   
        public QueueMessagesResponse() { }
        public QueueMessagesResponse(string status, string message, string executionTime)
        {
            Status = status;
            Message = message;
            ExecutionTime = executionTime;

        }

    }
}
