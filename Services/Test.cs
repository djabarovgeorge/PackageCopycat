using PackageCopycat.DbContexts;
using PackageCopycat.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageCopycat.Services
{
    public class Test
    {
        private DBContext _context;
        public Test(DBContext context)
        {
            this._context = context;
            AddUser();
        }
        public void AddUser()
        {
            _context.Add(new Package
            {
                PackageId = 1177,
                ClientName = "admin",
                UserName = "admin",
                Password = "password",
                QueueId = "214365"
            });
        }
    }
}
