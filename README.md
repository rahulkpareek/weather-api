# WeatherApi

A .NET Web API service for retrieving weather forecasts and current weather data using the Visual Crossing Weather API. This service provides structured weather information with location details and comprehensive forecast data.

## Features

- Current weather conditions for any city
- Weather forecasts for up to 7 days
- Location information including resolved addresses, coordinates, and timezone data
- Comprehensive weather data including temperature, humidity, precipitation, wind speed, and pressure
- Asynchronous operations for optimal performance
- Robust error handling with detailed exception messages
- Multiple endpoints for different use cases

## API Endpoints

### Get Current Weather
```
GET /api/weather/current/{city}
```
Returns the current weather conditions for the specified city.

### Get Weather Forecast
```
GET /api/weather/forecast/{city}?days={numberOfDays}
```
Returns weather forecast for the specified city. Default is 7 days, maximum is 7 days.

### Get Weather with Location Details
```
GET /api/weather/location/{city}?days={numberOfDays}
```
Returns weather forecast along with detailed location information including coordinates and timezone.

## Data Models

### WeatherForecast
```json
{
  "date": "2024-01-15",
  "temperatureC": 22.5,
  "temperatureMaxC": 25.0,
  "temperatureMinC": 18.0,
  "feelsLikeC": 23.0,
  "humidity": 65.0,
  "precipitation": 0.0,
  "precipitationProbability": 10.0,
  "windSpeed": 15.5,
  "pressure": 1013.2,
  "conditions": "Partly cloudy",
  "description": "Partly cloudy throughout the day",
  "icon": "partly-cloudy-day"
}
```

### WeatherResponse (with Location)
```json
{
  "location": {
    "latitude": 40.7128,
    "longitude": -74.0060,
    "resolvedAddress": "New York, NY, United States",
    "address": "New York",
    "timezone": "America/New_York"
  },
  "forecasts": [
    // Array of WeatherForecast objects
  ]
}
```

## Prerequisites

- .NET 6.0 or later
- Visual Crossing Weather API key
- Internet connection for API calls

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/WeatherApi.git
   cd WeatherApi
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure API settings**
   
   Update your `appsettings.json` or `appsettings.Development.json`:
   ```json
   {
     "WeatherApi": {
       "ApiKey": "YOUR_VISUAL_CROSSING_API_KEY",
       "BaseUrl": "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/"
     }
   }
   ```

   Alternatively, set environment variables:
   ```bash
   export WeatherApi__ApiKey="YOUR_VISUAL_CROSSING_API_KEY"
   export WeatherApi__BaseUrl="https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/"
   ```

## Getting a Visual Crossing API Key

1. Visit [Visual Crossing Weather](https://www.visualcrossing.com/weather-api)
2. Create a free account
3. Navigate to your account dashboard
4. Copy your API key
5. The free tier includes 1,000 API calls per day

## Usage

### Running the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

### Example Requests

**Get current weather for London:**
```bash
curl https://localhost:5001/api/weather/current/London
```

**Get 5-day forecast for New York:**
```bash
curl https://localhost:5001/api/weather/forecast/New%20York?days=5
```

**Get weather with location details:**
```bash
curl https://localhost:5001/api/weather/location/Tokyo
```

### Using with HttpClient

```csharp
var httpClient = new HttpClient();
var response = await httpClient.GetAsync("https://localhost:5001/api/weather/current/London");
var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();
```

## Configuration

### Dependency Injection Setup

The WeatherService is designed to be used with dependency injection:

```csharp
// In Program.cs or Startup.cs
services.AddHttpClient<WeatherService>();
services.AddScoped<WeatherService>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    var apiKey = configuration["WeatherApi:ApiKey"];
    var baseUrl = configuration["WeatherApi:BaseUrl"];
    return new WeatherService(httpClient, apiKey, baseUrl);
});
```

### Environment Variables

For production deployments, use environment variables:

- `WeatherApi__ApiKey`: Your Visual Crossing API key
- `WeatherApi__BaseUrl`: The Visual Crossing API base URL

## Error Handling

The service includes comprehensive error handling for:

- Network errors such as connection timeouts and DNS resolution failures
- API errors including invalid API keys, rate limiting, and service unavailable responses
- Data errors from invalid city names or malformed responses
- Request timeout errors with configurable HttpClient settings

All errors are wrapped in descriptive exception messages to facilitate debugging.

## Development

### Project Structure

```
WeatherApi/
├── Controllers/          # API controllers
├── Services/            # Business logic services
│   └── WeatherService.cs
├── Models/              # Data models
├── Program.cs           # Application entry point
├── appsettings.json     # Configuration
└── README.md
```

### Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -m 'Add new feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Open a Pull Request

## Testing

Run the tests using:

```bash
dotnet test
```

## Performance Considerations

- The service uses HttpClient with dependency injection for connection pooling
- All operations are asynchronous to prevent blocking
- API responses benefit from HTTP client-level caching
- Consider implementing response caching for production environments

## Rate Limiting

Visual Crossing API has the following rate limits:
- Free tier: 1,000 requests per day
- Consider implementing caching or request throttling for high-traffic applications

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [Visual Crossing Weather API](https://www.visualcrossing.com/) for providing weather data
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) for the web framework
- [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview) for JSON serialization

## Support

If you encounter issues or have questions:

1. Check the [Issues](https://github.com/yourusername/WeatherApi/issues) page
2. Create a new issue with detailed information about your problem
3. Include error messages, request/response examples, and environment details

---

**Important**: Keep your API key secure and never commit it to version control. Use environment variables or secure configuration management in production environments.
```

Now let's add and commit the professional README:

```bash
git add README.md
```

```bash
git commit -m "Add professional README documentation"
```

```bash
git push