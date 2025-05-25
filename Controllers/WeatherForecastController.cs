using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(string city = "Munich", int days = 7)
        {
            try
            {
                var forecasts = await _weatherService.GetForecastAsync(city, days);
                return Ok(forecasts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weather forecast for {City}", city);
                return StatusCode(500, "Error retrieving weather forecast. Please try again later.");
            }
        }
        
        [HttpGet("current/{city}", Name = "GetCurrentWeather")]
        public async Task<ActionResult<WeatherForecast>> GetCurrent(string city)
        {
            try
            {
                var forecast = await _weatherService.GetCurrentWeatherAsync(city);
                return Ok(forecast);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current weather for {City}", city);
                return StatusCode(500, "Error retrieving current weather. Please try again later.");
            }
        }

        [HttpGet("detailed/{city}", Name = "GetDetailedWeather")]
        public async Task<ActionResult<WeatherResponse>> GetDetailed(string city, int days = 7)
        {
            try
            {
                var weatherResponse = await _weatherService.GetWeatherWithLocationAsync(city, days);
                return Ok(weatherResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting detailed weather for {City}", city);
                return StatusCode(500, "Error retrieving detailed weather. Please try again later.");
            }
        }
    }
}