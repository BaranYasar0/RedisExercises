using StackExchange.Redis;

namespace RedisExampleApp.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        public RedisService(string url)
        {
            connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }

        public IDatabase GetDb(int db = 0)
        {
            return connectionMultiplexer.GetDatabase(db);
        }
    }
}
