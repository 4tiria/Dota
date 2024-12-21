namespace Dota.API.Statistics.RabbitMq.DLX;

public interface IStatisticsDlxService
{
    void Configure(string exchange, string routingKey, string dlxQueue);
}