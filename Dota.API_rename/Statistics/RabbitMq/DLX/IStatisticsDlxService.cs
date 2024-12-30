namespace Dota.API.Statistics.RabbitMq.DLX;

public interface IStatisticsDlxService
{
    void Consume();
}