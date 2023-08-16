using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisApp.Web.Models;

namespace RedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
       private readonly IDistributedCache _distributedCache;

       public ProductController(IDistributedCache distributedCache)
       {
           _distributedCache = distributedCache;
       }

       public async Task<IActionResult> Index()
        {
         DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(10));
         
            
            string jsonProduct = JsonConvert.SerializeObject(new Product { Id = 1, Name = "Kalem", Price = 100 });

           byte[] byteProduct=Encoding.UTF8.GetBytes(jsonProduct);

           await _distributedCache.SetAsync("name", byteProduct, options);
            //await _distributedCache.SetStringAsync("name", jsonroduct,options);

            return View();
        }

       public async Task<IActionResult> Show()
       {
          byte[] jsonProduct= await _distributedCache.GetAsync("name");

            Product p=JsonConvert.DeserializeObject<Product>(Encoding.UTF8.GetString(jsonProduct));
            ViewBag.v1=p.Name;
           return View();
       }

       public async Task<IActionResult> ImageCache()
       {
           string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/den.jpg");
           byte[] imageByte = await System.IO.File.ReadAllBytesAsync(imagePath);

           await _distributedCache.SetAsync("image", imageByte);
           return View();
       }

       public async Task<IActionResult> ImageUrl()
       {
           var imageBytes=await _distributedCache.GetAsync("image");

            var image=File(imageBytes,"image/jpeg");

           return image;
       }
    }
}
