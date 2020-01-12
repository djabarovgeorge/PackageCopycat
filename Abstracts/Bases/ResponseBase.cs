using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Abstracts.Bases
{
    public abstract class ResponseBase
    {
        public abstract string Status { get; set; }
        public abstract string Message { get; set; }
    }
}
