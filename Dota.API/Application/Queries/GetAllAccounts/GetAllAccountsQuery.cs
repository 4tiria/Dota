using MediatR;

namespace Dota.API.Queries.GetAllAccounts;

public class GetAllAccountsQuery : IRequest<IEnumerable<AccountDto>>
{
    
}