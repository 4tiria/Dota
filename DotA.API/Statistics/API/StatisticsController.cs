using Dota.API.Statistics.RabbitMq.Producers;
using Microsoft.AspNetCore.Mvc;

namespace Dota.API.Statistics.API;

[ApiController, Route("api/statistics")]
public class StatisticsController(IStatisticsProducerService statisticsProducerService)
{
    [HttpPatch("update")]
    public void Update()
    {
        statisticsProducerService.Produce();
    }
}