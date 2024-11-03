using Dota.API.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace Dota.API.Controllers;

[ApiController, Route("api/weather")]
public class WeatherController(IRabbitMQProducerService rabbitMQProducerService) : Controller
{
    private readonly IRabbitMQProducerService _rabbitMQProducerService = rabbitMQProducerService;

    [HttpGet]
    public void Forecast()
    {
        _rabbitMQProducerService.Produce();
    }
}
