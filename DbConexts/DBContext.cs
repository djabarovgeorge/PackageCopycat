using Microsoft.EntityFrameworkCore;
using PackageCopycat.Models.DBModels;

namespace PackageCopycat.DbContexts
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<Package> Packages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
