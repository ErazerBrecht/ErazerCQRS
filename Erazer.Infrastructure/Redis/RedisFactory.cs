namespace Erazer.Infrastructure.Redis
{
    public class RedisFactory : IFactory<IRedisClientsManager>
    {
        private readonly IOptions<RedisSettings> _options;
        private readonly ILogger<RedisFactory> _logger;

        public RedisFactory(IOptions<RedisSettings> options, ILogger<RedisFactory> logger)
        {
            _options = options;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new ArgumentNullException(options.Value.ConnectionString, "Connection string is required when setting up a connection with a 'Redis' server");

            _logger.LogInformation($"Building a connection to a 'Redis' server");
        }

        public IRedisClientsManager Build()
        {
            try
            {
                var pool = new RedisManagerPool(_options.Value.ConnectionString);

                using (var redis = pool.GetClient())
                {
                    if (!redis.Ping())
                        throw new Exception("Could not ping the redis server");
                }

                _logger.LogInformation($"Created a successful connection with the 'Redis' server");
                return pool;
            }
            catch (Exception)
            {
                _logger.LogCritical($"Could NOT create a successful connection with the 'Redis' server");
                throw;
            }
        }
    }
}
