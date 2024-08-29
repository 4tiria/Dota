using Domain.NoSql.Auth;
using Domain.NoSql.Auth.Models;
using Domain.NoSql.Auth.Models.Entities;
using MongoDB.Driver;

namespace MySqlToMongoMigrator;

public class AccountTableHandler(MongoDbContext mongoDbContext, AuthContext mySqlContext)
{
    public void Execute()
    {
        mongoDbContext.Accounts.DeleteMany(hero => true);

        var accounts = mySqlContext.Accounts.AsEnumerable().ToList();
        foreach (var account in accounts)
        {
            if (mongoDbContext.Accounts.Find(x => x.Id == account.Id) != null)
            {
                mongoDbContext.Accounts.DeleteOne(h => h.Id == account.Id);
            }

            mongoDbContext.Accounts.InsertOne(new Account()
            {
                Id = account.Id,
                AccessLevel = account.AccessLevel,
                //TODO: GridFS
                Avatar = account.Avatar,
                ConfirmationExpiration = account.ConfirmationExpiration,
                ConfirmationLink = account.ConfirmationLink,
                Email = account.Email,
                IsConfirmed = account.IsConfirmed,
                Password = account.Password,
                UserName = account.UserName,
            });
        }
    }
}
