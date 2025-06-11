using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Design;

namespace VisaChecker
{
    public class ProductContext : DbContext
    {

        public ProductContext(DbContextOptions<ProductContext> opts) : base(opts)
        {

        }
        public DbSet<Gov> Gov { get; set; }
    }

    public class ProductContextFactory : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
            optionsBuilder.UseSqlServer("Server=HOMEPC\\SQLEXPRESS;Database=sponsor;TrustServerCertificate=True;Trusted_Connection=True;");
            return new ProductContext(optionsBuilder.Options);
        }
    }
}
