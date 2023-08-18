using System.Text.Json;
using RedisExampleApp.Model;
using RedisExampleApp.Services;
using StackExchange.Redis;

namespace RedisExampleApp.Repository
{
    public class ProductRepositoryWithCache:IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private const string _cacheKey = "ProductList";

        public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public async Task<List<Product>> GetListAsync()
        {
            if(!await db.KeyExistsAsync(_cacheKey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();

            foreach (var item in (await db.HashGetAllAsync(_cacheKey)).ToList())
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);

                products.Add(product);
            }

            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            if (db.KeyExists(_cacheKey))
            {
                var product = await db.HashGetAsync(_cacheKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : new Product();
            }

            var productList = await LoadToCacheFromDbAsync();

            return productList.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var createdProduct = await _productRepository.CreateAsync(product);

            if (await db.KeyExistsAsync(_cacheKey))
                await db.HashSetAsync(_cacheKey, product.Id, JsonSerializer.Serialize<Product>(product));

            return createdProduct;
        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products=await _productRepository.GetListAsync();

            products.ForEach(x=>
            {
                db.HashSetAsync(_cacheKey, x.Id, JsonSerializer.Serialize(x));
            });

            return products;
        }

    }
}
