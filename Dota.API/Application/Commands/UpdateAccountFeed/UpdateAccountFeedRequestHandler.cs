using Dota.API.Commands.CreateAccount;
using MediatR;

namespace Dota.API.Commands.UpdateAccountFeed;

public class UpdateAccountFeedRequestHandler : IRequestHandler<CreateAccountRequest>
{
    public Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}