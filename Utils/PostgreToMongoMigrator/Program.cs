using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlToMongoMigrator;
using NoSql;

Console.WriteLine("Запущен мигратор с PostreSql на MongoDb");

var connectionString = "Server=localhost;user=root;database=dota-auth;password=root";
var optionsBuilder = new DbContextOptionsBuilder<ApiContext>();
optionsBuilder.UseMySQL(connectionString);
var mysqlDbContext = new ApiContext(optionsBuilder.Options);

var mongoDbContext = new MongoDbContext(Options.Create(new MongoDbSettings()
{
    ConnectionURI = "mongodb://localhost:27017",
    DatabaseName = "dota-auth",
}));

//new HeroTableHandler(mongoDbContext, mysqlDbContext).Execute();
//new MatchTableHandler(mongoDbContext, mysqlDbContext).Execute();
new AccountTableHandler(mongoDbContext, mysqlDbContext).Execute();


Console.WriteLine("Мигратор успешно завершился");
Console.ReadLine();
