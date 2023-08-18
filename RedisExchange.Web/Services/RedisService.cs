using StackExchange.Redis;

namespace RedisExchange.Web.Services
{
    public class RedisService
    {
        private readonly string redisHost;
        private readonly string redisPort;
        private ConnectionMultiplexer redis;
        private readonly IDatabase db;

        public RedisService(IConfiguration configuration)
        {
            this.redisHost = configuration["RedisSettings:Host"];
            this.redisPort = configuration["RedisSettings:Port"];
            
        }

        public void Connect()
        {
            var configString = $"{redisHost}:{redisPort}";

            redis=ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDb(int db = 0)
        {
            return redis.GetDatabase(db);
        }
    }
}
