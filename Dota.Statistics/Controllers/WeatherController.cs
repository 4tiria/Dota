using Dota.Statistics.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dota.Statistics.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController(IWeatherService weatherService) : Controller
    {
        private readonly IWeatherService _weatherService = weatherService;

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _weatherService.Forecast();
        }
    }
}
