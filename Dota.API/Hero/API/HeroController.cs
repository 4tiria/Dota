using AutoMapper;
using Domain.Mongo.API;
using Dota.API.Helpers;
using Dota.API.Hero.RabbitMq.Consumers;
using Dota.API.Models.DTO;
using Dota.API.Models.EntitiesJs;
using Dota.API.Models.FilterModels;
using Dota.API.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dota.API.Hero.API
{
    [ApiController, Route("api/hero")]
    public class HeroController(
        MongoDbContext apiContext,
        IMapper mapper,
        IHeroProducerService heroProducerService)
        : Controller
    {
        [HttpGet("list")]
        public IActionResult GetHeroes()
        {
            return Ok(apiContext.Heroes.Find(hero => true).ToEnumerable().Select(mapper.Map<HeroJs>));
        }
        
        [HttpGet("list/{tagName}")]
        public IActionResult GetHeroesByTag(string tagName)
        {
            tagName = tagName.ToLower();
            return Ok(apiContext.Heroes
                .Find(hero => hero.Tags.Any(t => t == tagName))
                .ToList()
                .Select(mapper.Map<TagJs>)
            );
        }

        [HttpPost("list/filter")]
        public IActionResult GetFilteredHeroes([FromBody] HeroFilterModel filterOptions)
        {
            var result = apiContext.Heroes.Find(hero => true).ToEnumerable();

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
                result = result.Where(hero => hero.Name.StartsWith(lowerNameFilter, StringComparison.CurrentCultureIgnoreCase));
            }

            return Ok(result.ToList().Select(mapper.Map<HeroJs>));
        }

        [HttpPost("byName")]
        public IActionResult GetHeroByName([FromBody] CamelCaseNameJs camelCaseNameJs)
        {
            var hero = apiContext.Heroes
                .Find(h => h.Name == camelCaseNameJs.FromCamelCase());

            if (hero is null)
                return NotFound();

            return Ok(mapper.Map<HeroJs>(hero));
        }


        [HttpGet("{id}")]
        public IActionResult GetHeroById(string id)
        {
            var hero = apiContext.Heroes
                .Find(h => h.Id == ObjectId.Parse(id));

            if (hero is null)
                return NotFound();

            return Ok(mapper.Map<HeroJs>(hero));
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] HeroJs heroJs)
        {
            var heroInContext = apiContext.Heroes
                .Find(hero => hero.Id == ObjectId.Parse(heroJs.Id)).Single();

            if (heroInContext is null)
                return NotFound();

            heroInContext.Name = heroJs.Name;
            heroInContext.MainAttribute = heroJs.MainAttribute;
            heroInContext.AttackType = heroJs.AttackType;
            heroInContext.Tags = heroJs.Tags.Select(Enum.GetName).ToList();

            return Ok();
        }

        [HttpGet("getWinrate")]
        public void GetWinrate(ObjectId heroId)
        {
            heroProducerService.Produce(heroId);
        }
    }
}