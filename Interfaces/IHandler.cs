using PackageCopycat.Abstracts.Bases;
using PackageCopycat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IHandler
    {
        Type Key { get; }
        Task<ResponseBase> HandleAsync(RequestBase request);
    }
}
