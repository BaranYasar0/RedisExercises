using Microsoft.AspNetCore.Mvc;
using RedisExchange.Web.Services;
using StackExchange.Redis;

namespace RedisExchange.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            this.redisService = redisService;
            this.db = redisService.GetDb(1);
        }
        
        public IActionResult Index()
        {

            db.StringSet("name", "Sikici Baro");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");
            db.StringIncrement("ziyaretci", 10);
            if(value.HasValue)
                ViewBag.name = value.ToString();

            return View();
        }
    }
}
