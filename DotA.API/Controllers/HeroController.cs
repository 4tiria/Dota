using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotA.API.Helpers;
using DotA.API.Models;
using DotA.API.Models.Entities;
using DotA.API.Models.FilterModels;
using DotA.API.Models.JsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/hero")]
    public class HeroController : Controller
    {
        //todo: add and change image
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
        
        [HttpPost("list/filter")]
        public IActionResult GetFilteredHeroes([FromBody] HeroFilterModel filterOptions)
        {
            var result = _apiContext.Heroes.AsEnumerable();

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
                result = result.ToList().Where(x => x.Tags
                    .Select(t => t.Name)
                    .ToHashSet().IsSupersetOf(filterOptions.Tags.Select(t => t.Name)));
            }

            if (filterOptions.Name.Length > 0)
            {
                var lowerNameFilter = filterOptions.Name.ToLower().TrimStart().TrimEnd();
                result = result.Where(x => x.Name.ToLower().StartsWith(lowerNameFilter));
            }
            
            return Ok(result.ToList());
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


        [HttpGet("{id:int}")]
        public IActionResult GetHeroById(int id)
        {
            var hero = _apiContext.Heroes
                .FirstOrDefault(x => x.Id == id);

            if (hero is null)
                return NotFound();

            return Ok(hero);
        }

        [HttpPatch]
        public IActionResult Update([FromBody] Hero hero)
        {
            var heroInContext = _apiContext.Heroes.Find(hero.Id);

            if (heroInContext is null)
                return NotFound();

            heroInContext.Name = hero.Name;
            heroInContext.MainAttribute = hero.MainAttribute;
            heroInContext.AttackType = hero.AttackType;
            var (toDelete, toAdd) = GetListsToModifyHeroTag(
                heroInContext.Tags.Select(x => x.Name).ToList(),
                hero.Tags.Select(x => x.Name).ToList());

            foreach (var stringTag in toDelete)
                heroInContext.Tags.Remove(_apiContext.Tags.Find(stringTag));

            foreach (var stringTag in toAdd)
                heroInContext.Tags.Add(_apiContext.Tags.Find(stringTag));

            _apiContext.SaveChanges();

            return Ok();
        }

        [HttpPatch("{id:int}/image")]
        public IActionResult AddOrChangeHeroImage(int id)
        {
            var files = Request.Form.Files;
            if (!files.Any())
            {
                return BadRequest();
            }
            var file = files[0];
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            var heroImage = _apiContext.HeroImages.Find(id);
            if (heroImage is null)
            {
                _apiContext.HeroImages.Add(new HeroImage() { Bytes = fileBytes, HeroId = id });
            }
            else
            {
                heroImage.Bytes = fileBytes;
            }
     
            _apiContext.SaveChanges();

            return Ok();
        }

        [HttpGet("{id:int}/image")]
        public IActionResult GetHeroImage(int id)
        {
            var heroImage = _apiContext.HeroImages.Find(id);
            if (heroImage is null)
            {
                return NotFound();
            }

            return File(heroImage.Bytes, "image/png");
        }

        [HttpPost("empty")]
        public IActionResult AddEmptyHero()
        {
            var hero = new Hero
            {
                Name = "New Hero"
            };

            _apiContext.Heroes.Add(hero);
            _apiContext.SaveChanges();
            return Ok(hero);
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody] Hero hero)
        {
            var heroInContext = _apiContext.Heroes.Find(hero.Id);
            if (heroInContext is null)
            {
                return NotFound();
            }

            _apiContext.Heroes.Remove(heroInContext);
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