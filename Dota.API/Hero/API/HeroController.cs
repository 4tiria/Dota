using AutoMapper;
using Domain.Mongo.API;
using Dota.API.Helpers;
using Dota.API.Models.DTO;
using Dota.API.Models.EntitiesJs;
using Dota.API.Models.FilterModels;
using Dota.API.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Dota.API.Hero.API;

[ApiController]
[Route("api/hero")]
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

    [HttpPost("list/filter")]
    public IActionResult GetFilteredHeroes([FromBody] HeroFilterModel filterOptions)
    {
        var result = mapper.Map<List<HeroJs>>(apiContext.Heroes.Find(hero => true)).AsEnumerable();

        if (filterOptions.AttackType != "All")
        {
            result = result.Where(hero => hero.AttackType == filterOptions.AttackType);
        }

        if (filterOptions.MainAttribute != "All")
        {
            result = result.Where(x => x.MainAttribute == filterOptions.MainAttribute);
        }

        if (filterOptions.Roles.Any())
        {
            result = result
                .ToList()
                .Where(x => x.Roles
                    .ToHashSet()
                    .IsSupersetOf(filterOptions.Roles));
        }

        if (filterOptions.Name.Length > 0)
        {
            var lowerNameFilter = filterOptions.Name.ToLower().TrimStart().TrimEnd();
            result = result.Where(hero =>
                hero.Name.StartsWith(lowerNameFilter, StringComparison.CurrentCultureIgnoreCase));
        }

        return Ok(result.ToList().Select(mapper.Map<HeroJs>));
    }

    [HttpPost("byName")]
    public IActionResult GetHeroByName([FromBody] CamelCaseNameJs camelCaseNameJs)
    {
        var hero = apiContext.Heroes
            .Find(h => h.LocalizedName == camelCaseNameJs.FromCamelCase());

        if (hero is null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<HeroJs>(hero));
    }

    [HttpGet("{id}")]
    public IActionResult GetHeroById(string id)
    {
        var hero = apiContext.Heroes
            .Find(h => h.Id == ObjectId.Parse(id));

        if (hero is null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<HeroJs>(hero));
    }

    [HttpGet("getWinrate")]
    public void GetWinrate(ObjectId heroId)
    {
        heroProducerService.Produce(heroId);
    }
}