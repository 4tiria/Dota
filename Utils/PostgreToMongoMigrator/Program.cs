using DataAccess;
using Domain.NoSql.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlToMongoMigrator;
using NoSql;

Console.WriteLine("Запущен мигратор с PostreSql на MongoDb");

var connectionString = "Server=localhost;user=root;database=dota-auth;password=root";
var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
optionsBuilder.UseMySQL(connectionString);
var mysqlDbContext = new AuthContext(optionsBuilder.Options);

//var mongoDbContext = new Domain.NoSql.MongoDbContext(Options.Create(new MongoDbSettings()
//{
//    ConnectionURI = "mongodb://localhost:27017",
//    DatabaseName = "dota",
//}));

var mongoDbContextAuth = new Domain.NoSql.Auth.MongoDbContext(
    Options.Create(new Domain.NoSql.Auth.MongoDbSettings()
{
    ConnectionURI = "mongodb://localhost:27017",
    DatabaseName = "dota-auth",
}));

//new HeroTableHandler(mongoDbContext, mysqlDbContext).Execute();
//new MatchTableHandler(mongoDbContext, mysqlDbContext).Execute();
new AccountTableHandler(mongoDbContextAuth, mysqlDbContext).Execute();
new RefreshTokenTableHandler(mongoDbContextAuth, mysqlDbContext).Execute();


Console.WriteLine("Мигратор успешно завершился");
Console.ReadLine();
