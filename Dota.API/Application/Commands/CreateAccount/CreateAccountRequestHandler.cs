using AutoMapper;
using Domain.Mongo.API;
using MediatR;

namespace Dota.API.Commands.CreateAccount;

public class CreateAccountRequestHandler(MongoDbContext apiContext, IMapper mapper) : IRequestHandler<CreateAccountRequest>
{
    public async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        await apiContext.Accounts.InsertOneAsync(mapper.Map<Domain.Mongo.API.Account>(request), cancellationToken: cancellationToken);
    }
}
