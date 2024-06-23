using AutoMapper;
using DotA.API.Helpers;
using DotA.API.Models.DTO;
using DotA.API.Models.EntitiesJs;
using DotA.API.Models.FilterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NoSql;
using NoSql.Models;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/hero")]
    public class HeroController : Controller
    {
        private readonly MongoDbContext _context;
        private readonly IMapper _mapper;

        public HeroController(MongoDbContext apiContext, IMapper mapper)
        {
            _context = apiContext;
            _mapper = mapper;
        }
        
        [HttpGet("list")]
        public IActionResult GetHeroes()
        {
            return Ok(_context.Heroes.Find(hero => true).ToEnumerable().Select(_mapper.Map<HeroJs>));
        }
        
        [HttpGet("list/{tagName}")]
        public IActionResult GetHeroesByTag(string tagName)
        {
            tagName = tagName.ToLower();
            return Ok(_context.Heroes
                .Find(hero => hero.Tags.Any(t => t == tagName))
                .ToList()
                .Select(_mapper.Map<TagJs>)
            );
        }

        [HttpPost("list/filter")]
        public IActionResult GetFilteredHeroes([FromBody] HeroFilterModel filterOptions)
        {
            var result = _context.Heroes.Find(hero => true).ToEnumerable();

            if (filterOptions.AttackType != "All")
            {
                result = result.Where(x => x.AttackType == filterOptions.AttackType);
            }

            if (filterOptions.MainAttribute != "All")
            {
                result = result.Where(x => x.MainAttribute == filterOptions.MainAttribute);
            }

            if (filterOptions.Tags.Any())
            {
                result = result
                    .ToList()
                    .Where(x => x.Tags
                        .ToHashSet()
                        .IsSupersetOf(filterOptions.Tags.Select(Enum.GetName)));
            }

            if (filterOptions.Name.Length > 0)
            {
                var lowerNameFilter = filterOptions.Name.ToLower().TrimStart().TrimEnd();
                result = result.Where(hero => hero.Name.ToLower().StartsWith(lowerNameFilter));
            }

            return Ok(result.ToList().Select(_mapper.Map<HeroJs>));
        }

        [HttpPost("byName")]
        public IActionResult GetHeroByName([FromBody] CamelCaseNameJs camelCaseNameJs)
        {
            var hero = _context.Heroes
                .Find(h => h.Name == camelCaseNameJs.FromCamelCase());

            if (hero is null)
                return NotFound();

            return Ok(_mapper.Map<HeroJs>(hero));
        }


        [HttpGet("{id:int}")]
        public IActionResult GetHeroById(int id)
        {
            var hero = _context.Heroes
                .Find(h => h.Id == id);

            if (hero is null)
                return NotFound();

            return Ok(_mapper.Map<HeroJs>(hero));
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] HeroJs heroJs)
        {
            var heroInContext = _context.Heroes
                .Find(hero => hero.Id == heroJs.Id).Single();

            if (heroInContext is null)
                return NotFound();

            heroInContext.Name = heroJs.Name;
            heroInContext.MainAttribute = heroJs.MainAttribute;
            heroInContext.AttackType = heroJs.AttackType;
            heroInContext.Tags = heroJs.Tags.Select(Enum.GetName).ToList();

            return Ok();
        }

        [HttpPatch("{id:int}/image")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOrChangeHeroImage(int id)
        {
            var files = Request.Form.Files;
            if (!files.Any())
                return BadRequest();

            var bytes = files[0].ToByteArray();
            _context.Heroes.UpdateOne(h => h.Id == id, Builders<Hero>.Update.Set(h => h.Image, bytes));

            return Ok();
        }

        [HttpPost("empty")]
        public IActionResult AddEmptyHero()
        {
            var hero = new Hero
            {
                Name = "New Hero"
            };

            _context.Heroes.InsertOne(hero);
            return Ok(_mapper.Map<HeroJs>(hero));
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromBody] HeroJs heroJs)
        {
            var heroInContext = _context.Heroes.DeleteOne(hero => hero.Id == heroJs.Id);
            return Ok();
        }

        private (List<string> ToDelete, List<string> ToAdd) GetListsToModifyHeroTag(List<string> before,
            List<string> after)
        {
            var setBefore = new HashSet<string>(before);
            var setAfter = new HashSet<string>(after);
            var toDelete = setBefore.Except(setAfter).ToList();
            var toAdd = setAfter.Except(setBefore).ToList();
            return (toDelete, toAdd);
        }
    }
}