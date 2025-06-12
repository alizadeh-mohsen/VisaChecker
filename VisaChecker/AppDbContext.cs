using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Design;

namespace VisaChecker
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {

        }
        public DbSet<Gov> Gov { get; set; }
    }

    public class ProductContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=HOMEPC\\SQLEXPRESS;Database=sponsor;TrustServerCertificate=True;Trusted_Connection=True;");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
