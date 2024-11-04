namespace Domain.NoSql.Repositories.Migrations;

public interface IMigrationRepository
{
    public int GetCurrentDbVersion();
}