using DotA.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/tag")]
    public class TagController : Controller
    {
        private readonly ApiContext _apiContext;

        public TagController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_apiContext.Tags);
        }
    }
}