using PackageCopycat.Abstracts.Bases;
using PackageCopycat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Abstracts
{
    public abstract class HandlerBase<T> : IHandler where T : RequestBase
    {
        public abstract Type Key { get; }

        public async Task<ResponseBase> HandleAsync(RequestBase request) => await HandleInternalAsync((T)request);

        protected abstract Task<ResponseBase> HandleInternalAsync(T request);
    }
}
