
using Dota.Statistics.Controllers;
using Microsoft.Extensions.Logging;

namespace Dota.Statistics.Services;

public class WeatherService(ILogger<WeatherService> logger) : IWeatherService
{
    private static readonly string[] Summaries =
      [
          "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherService> _logger = logger;

    public IEnumerable<WeatherForecast> Forecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
           .ToArray();
    }
}
