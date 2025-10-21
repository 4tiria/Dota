using AutoMapper;
using Dota.API.Account.DTO;
using Dota.API.Commands.CreateAccount;
using Dota.API.Commands.UpdateAccountFeed;
using MassTransit;
using MediatR;

namespace Dota.API.BackgroundWorkers;

public class AccountsConsumer(IMediator mediator, IMapper mapper) : IConsumer<AccountCreated>
{
    public async Task Consume(ConsumeContext<AccountCreated> context)
    {
        _ = mediator.Publish(mapper.Map<UpdateAccountFeedRequest>(context.Message));
        await mediator.Send(mapper.Map<CreateAccountRequest>(context.Message));
    }
}