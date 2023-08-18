using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Model;

namespace RedisExampleApp.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await context.Products.AddAsync(product);
            var created = await context.SaveChangesAsync();
            return product;
        }
    }
}
