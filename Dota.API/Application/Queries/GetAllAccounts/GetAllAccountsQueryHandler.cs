using AutoMapper;
using Domain.Mongo.API;
using MediatR;
using MongoDB.Driver;

namespace Dota.API.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler(MongoDbContext apiContext, IMapper mapper) : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
{
    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await apiContext.Accounts
            .Find(account => account.CreationDate != default)
            .ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<AccountDto>>(accounts);
    }
}