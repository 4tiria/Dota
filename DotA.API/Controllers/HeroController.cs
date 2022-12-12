using System.Collections.Generic;
using System.Linq;
using DotA.API.Helpers;
using DotA.API.Models;
using DotA.API.Models.JsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        [HttpGet("list/{tagName}")]
        public IActionResult GetHeroesByTag(string tagName)
        {
            tagName = tagName.ToLower();
            var tag = _apiContext.Tags.First(x => x.Name.ToLower() == tagName);
            return Ok(_apiContext.Heroes.Where(x => x.Tags.Any(t => t.Name == tag.Name)));
        }

        [HttpGet("list")]
        public IActionResult GetHeroes()
        {
            return Ok(_apiContext.Heroes);
        }

        [HttpPost("byName")]
        public IActionResult GetHeroByName([FromBody] CamelCaseNameJs camelCaseNameJs)
        {
            var hero = _apiContext.Heroes
                .FirstOrDefault(x => x.Name == camelCaseNameJs.FromCamelCase());

            if (hero is null)
                return NotFound();

            return Ok(hero);
        }


        [HttpGet("/{id:int}")]
        public IActionResult GetHeroById(int id)
        {
            var hero = _apiContext.Heroes
                .FirstOrDefault(x => x.Id == id);

            if (hero is null)
                return NotFound();

            return Ok(hero);
        }

        [HttpPost]
        public IActionResult AddOrUpdate([FromBody] Hero hero)
        {
            var heroInContext = _apiContext.Heroes
                .Find(hero.Id);

            if (heroInContext is null)
            {
                _apiContext.Heroes.Add(hero);
            }
            else
            {
                heroInContext.Name = hero.Name;
                heroInContext.MainAttribute = hero.MainAttribute;
                var (toDelete, toAdd) = GetListsToModifyHeroTag(
                    heroInContext.Tags.Select(x => x.Name).ToList(),
                    hero.Tags.Select(x => x.Name).ToList());

                foreach (var stringTag in toDelete)
                    heroInContext.Tags.Remove(_apiContext.Tags.Find(stringTag));
                
                foreach (var stringTag in toAdd)
                    heroInContext.Tags.Add(_apiContext.Tags.Find(stringTag));
            }

            _apiContext.SaveChanges();

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