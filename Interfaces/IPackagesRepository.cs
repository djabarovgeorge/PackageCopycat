using PackageCopycat.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Interfaces
{
    public interface IPackagesRepository
    {
        Task<List<Package>> Get();
        Task<Package> Get(int packageId);
    }
}
