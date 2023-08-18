using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.Model;
using RedisExampleApp.Repository;
using RedisExampleApp.Services;
using StackExchange.Redis;

namespace RedisExampleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly RedisService redisService;

        public ProductsController(IProductRepository productRepository,RedisService redisService)
        {
            this.productRepository = productRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productRepository.GetListAsync();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            await productRepository.CreateAsync(product);
            return Created(string.Empty, product);
        }

    }
}
