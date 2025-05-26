namespace WeatherApi.Configuration
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int DefaultExpirationMinutes { get; set; } = 30;
    }
}