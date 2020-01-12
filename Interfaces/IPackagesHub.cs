using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IPackagesHub
    {
        Task Excepion(string exceptionDetails);
        Task MessageReceived(string message);
        Task PackageFound(string userName);
        Task PackageNotFound();
    }
}
