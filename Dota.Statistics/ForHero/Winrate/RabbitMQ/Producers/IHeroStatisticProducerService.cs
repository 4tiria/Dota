namespace Dota.Statistics.ForHero.Winrate.RabbitMQ.Producers;

public interface IHeroStatisticProducerService
{
    void ProduceGetStatisticsResponse(Domain.Mongo.Statistics.Models.Entities.HeroStatistic heroStatistic);
}
