using RedisExampleApp.Model;

namespace RedisExampleApp.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetListAsync();

        Task<Product> GetByIdAsync(int id);

        Task<Product> CreateAsync(Product product);


    }
}
