using MongoDB.Bson;

namespace Dota.API.RabbitMQ;

public interface IHeroProducerService
{
    void Produce(ObjectId heroId);
}
