using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Models
{
    public class QueueMessageEventArgs : EventArgs
    {
        public string QueueMessage{ get; set; }
    }
}
