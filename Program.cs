using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherApi.Services;

namespace WeatherApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure HttpClient
            builder.Services.AddHttpClient();

            // Register WeatherService
            builder.Services.AddSingleton(provider => 
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                
                // These values should be stored in configuration
                var apiKey = builder.Configuration["VisualCrossing:ApiKey"] ?? "YOUR_API_KEY";
                var apiBaseUrl = builder.Configuration["VisualCrossing:BaseUrl"] ?? "https://api.example.com/weather";                
                return new WeatherService(httpClient, apiKey, apiBaseUrl);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}