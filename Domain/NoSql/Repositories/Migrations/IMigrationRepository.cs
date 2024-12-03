namespace Domain.NoSql.Repositories.Migrations;

public interface IMigrationRepository
{
    int GetCurrentDbVersion();
    void Add(int version, string description = null);
}