using System.Linq;
using AutoMapper;
using DataAccess;
using DataAccess.Models;
using DotA.API.Models;
using DotA.API.Models.EntitiesJs;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/match")]
    public class MatchController: Controller
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public MatchController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _context.Matches;
            if (!matches.Any())
                return NoContent();
            
            return Ok(matches
                .ToList()
                .Select(x => _mapper.Map<MatchJs>(x))
                .OrderByDescending(x => x.End)
                .ToList());
        }
    }
}