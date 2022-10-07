using System.Linq;
using DotA.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
  [ApiController, Route("api/hero")]
  public class HeroController : Controller
  {
    private readonly ApiContext _apiContext;

    public HeroController(ApiContext apiContext)
    {
      _apiContext = apiContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
      return Ok(_apiContext.Heroes);
    }

    [HttpGet("tag/{tag}")]
    public IActionResult GetHeroesByTag(string tag)
    {
      return Ok(_apiContext.Tags.First(x => x.Name == tag).Heroes);
    }
  }
}
