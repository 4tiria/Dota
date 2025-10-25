using Dota.API.Queries.GetAllAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dota.API.Presentation.Account.API;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IMediator mediator) : Controller
{
    [HttpGet("getAll")]
    //TODO: защититься от DDoS
    //TODO: вспомнить, зачем я это написал
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
    {
        var result = await mediator.Send(new GetAllAccountsQuery());
        return result.ToList();
    }
}