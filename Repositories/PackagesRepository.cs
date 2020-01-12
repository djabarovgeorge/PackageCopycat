using Microsoft.EntityFrameworkCore;
using PackageCopycat.DbContexts;
using PackageCopycat.Interfaces;
using PackageCopycat.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Repositories
{
    public class PackagesRepository : IPackagesRepository
    {
        private readonly DBContext _context;

        public PackagesRepository(DBContext context)
        {
            this._context = context;
        }

        public async Task<List<Package>> Get()
        {
            return await _context.Packages.ToListAsync();
        }

        public async Task<Package> Get(int packageId)
        {
            return await _context.Packages.Where(p => p.PackageId == packageId).FirstOrDefaultAsync();
        }
    }
}
