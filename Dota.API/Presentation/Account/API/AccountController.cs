using Dota.API.Account.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Dota.API.Account.API;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    [HttpGet("add")]
    //TODO: защититься от DDoS
    public IActionResult GetHeroes([FromBody] AccountCreated account)
    {
        throw new NotImplementedException();
    }
}