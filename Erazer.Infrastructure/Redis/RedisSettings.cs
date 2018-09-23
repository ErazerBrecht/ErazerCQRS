namespace Erazer.Infrastructure.Redis
{
    public class RedisSettings
    {
        public string ConnectionString { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
