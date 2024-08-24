using Dota.Statistics.RabbitMQ;
using Dota.Statistics.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dota.Statistics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IWeatherService weatherService) : ControllerBase
    {
        private readonly IWeatherService _weatherService = weatherService;

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherService.Forecast();
        }
    }
}
