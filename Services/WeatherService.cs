using System.Net.Http.Json;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiBaseUrl;

        public WeatherService(HttpClient httpClient, string apiKey, string apiBaseUrl)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _apiBaseUrl = apiBaseUrl;
        }

        public async Task<WeatherForecast> GetCurrentWeatherAsync(string city)
        {
            try
            {
                var response = await GetForecastAsync(city, 1);
                return response.FirstOrDefault() ?? throw new Exception("No weather data found");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching current weather data: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(string city, int days = 7)
        {
            try
            {
                // Construct URL for Visual Crossing API
                // Format: https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{location}?unitGroup=metric&key={apikey}&contentType=json
                var url = $"{_apiBaseUrl}{Uri.EscapeDataString(city)}?unitGroup=metric&key={_apiKey}&contentType=json";
                
                //Console.WriteLine($"Making request to: {url}"); // Debug log (remove API key from log)
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API request failed with status {response.StatusCode}: {errorContent}");
                }
                
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {responseContent.Substring(0, Math.Min(500, responseContent.Length))}..."); // Debug log
                
                var apiResponse = await response.Content.ReadFromJsonAsync<VisualCrossingApiResponse>();
                
                if (apiResponse?.Days == null || !apiResponse.Days.Any())
                    throw new Exception("No weather data found in API response");

                return MapToWeatherForecasts(apiResponse.Days.Take(days));
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error fetching forecast data: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timeout fetching forecast data: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching forecast data: {ex.Message}", ex);
            }
        }

        public async Task<WeatherResponse> GetWeatherWithLocationAsync(string city, int days = 7)
        {
            try
            {
                var url = $"{_apiBaseUrl}{Uri.EscapeDataString(city)}?unitGroup=metric&key={_apiKey}&contentType=json";

                //Console.WriteLine($"Making request to: {url}"); // Debug log (remove API key from log)
                
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API request failed with status {response.StatusCode}: {errorContent}");
                }
                
                var apiResponse = await response.Content.ReadFromJsonAsync<VisualCrossingApiResponse>();
                
                if (apiResponse?.Days == null)
                    throw new Exception("Invalid API response - no days data");

                return new WeatherResponse
                {
                    Location = new WeatherLocation
                    {
                        Latitude = apiResponse.Latitude,
                        Longitude = apiResponse.Longitude,
                        ResolvedAddress = apiResponse.ResolvedAddress ?? string.Empty,
                        Address = apiResponse.Address ?? string.Empty,
                        Timezone = apiResponse.Timezone ?? string.Empty
                    },
                    Forecasts = MapToWeatherForecasts(apiResponse.Days.Take(days)).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching weather data with location: {ex.Message}", ex);
            }
        }

        private IEnumerable<WeatherForecast> MapToWeatherForecasts(IEnumerable<DayForecast> days)
        {
            return days.Select(day => new WeatherForecast
            {
                Date = DateOnly.ParseExact(day.DateTime, "yyyy-MM-dd"),
                TemperatureC = day.Temp,
                TemperatureMaxC = day.TempMax,
                TemperatureMinC = day.TempMin,
                FeelsLikeC = day.FeelsLike,
                Humidity = day.Humidity,
                Precipitation = day.Precip,
                PrecipitationProbability = day.PrecipProb,
                WindSpeed = day.WindSpeed,
                Pressure = day.Pressure,
                Conditions = day.Conditions ?? string.Empty,
                Description = day.Description ?? string.Empty,
                Icon = day.Icon ?? string.Empty
            });
        }
    }

    // API Response Models for Visual Crossing Weather API
    public class VisualCrossingApiResponse
    {
        public int QueryCost { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ResolvedAddress { get; set; }
        public string? Address { get; set; }
        public string? Timezone { get; set; }
        public double TzOffset { get; set; }
        public List<DayForecast> Days { get; set; } = new();
    }

    public class DayForecast
    {
        public string DateTime { get; set; } = string.Empty;
        public long DatetimeEpoch { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public double Temp { get; set; }
        public double FeelsLikeMax { get; set; }
        public double FeelsLikeMin { get; set; }
        public double FeelsLike { get; set; }
        public double Humidity { get; set; }
        public double Precip { get; set; }
        public double PrecipProb { get; set; }
        public double WindSpeed { get; set; }
        public double Pressure { get; set; }
        public string? Conditions { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }
}