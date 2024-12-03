namespace Domain.Mongo.API.Repositories.Migrations;

public interface IMigrationRepository
{
    int GetCurrentDbVersion();
    void Add(int version, string description = null);
}