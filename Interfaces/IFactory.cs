using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IFactory
    {
        IHandler GetHandler(Type key);
    }
}
