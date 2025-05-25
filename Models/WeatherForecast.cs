namespace WeatherApi.Models
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public double TemperatureC { get; set; }
        public double TemperatureMaxC { get; set; }
        public double TemperatureMinC { get; set; }
        public double FeelsLikeC { get; set; }
        public double Humidity { get; set; }
        public double Precipitation { get; set; }
        public double PrecipitationProbability { get; set; }
        public double WindSpeed { get; set; }
        public double Pressure { get; set; }
        public string Conditions { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        
        // Computed property for Fahrenheit
        public double TemperatureF => (TemperatureC * 9 / 5) + 32;
        public double TemperatureMaxF => (TemperatureMaxC * 9 / 5) + 32;
        public double TemperatureMinF => (TemperatureMinC * 9 / 5) + 32;
        public double FeelsLikeF => (FeelsLikeC * 9 / 5) + 32;
    }

    public class WeatherLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ResolvedAddress { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Timezone { get; set; } = string.Empty;
    }

    // This is the WeatherResponse class - it combines location info with forecasts
    public class WeatherResponse
    {
        public WeatherLocation Location { get; set; } = new();
        public List<WeatherForecast> Forecasts { get; set; } = new();
    }
}