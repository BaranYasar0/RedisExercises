using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>().HasData(
                new Product(1, "kalem", 100),
                new Product(2, "Defter", 200),
                new Product(3, "silgi", 300)
            );


            base.OnModelCreating(modelBuilder);
        }
    }

}
