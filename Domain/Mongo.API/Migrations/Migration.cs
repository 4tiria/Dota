using MongoDB.Driver;

namespace Domain.Mongo.API.Migration;

public interface IMigration
{
    /// <summary>
    ///     Номер миграции
    /// </summary>
    int Version { get; }

    /// <summary>
    ///     Выполнить миграцию
    /// </summary>
    /// <param name="database"></param>
    /// <param name="session"></param>
    void Upgrade(MongoDbContext database, IClientSessionHandle session);
}