using Dota.API.Centrifugo;
using Microsoft.AspNetCore.Mvc;

namespace Dota.API.TestCentrifugo;

[ApiController]
[Route("api/centrifugo")]
public class TestCentrifugoController(ICentrifugoService centrifugoService) : Controller
{
    [HttpGet("token")]
    public IActionResult GetToken()
    {
        return Ok(centrifugoService.GenerateCentrifugoToken("admin"));
    }
    
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        await centrifugoService.PublishToCentrifugoAsync("channel1", "Hear me now, you, Demigods");
        return Ok();
    }
}