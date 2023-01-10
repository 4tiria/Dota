using System.Linq;
using DotA.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/match")]
    public class MatchController: Controller
    {
        private readonly ApiContext _context;

        public MatchController(ApiContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _context.Matches;
            if (!matches.Any())
                return NoContent();
            
            return Ok(matches.OrderByDescending(x => x.End).ToList());
        }
    }
}