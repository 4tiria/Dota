using AutoMapper;
using DataAccess;
using DataAccess.Models;
using DotA.API.Helpers;
using DotA.API.Models.DTO;
using DotA.API.Models.EntitiesJs;
using DotA.API.Models.FilterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/hero")]
    public class HeroController : Controller
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;

        public HeroController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }
        
        [HttpGet("list")]
        public IActionResult GetHeroes()
        {
            return Ok(_apiContext.Heroes.ToList().Select(_mapper.Map<HeroJs>));
        }
        
        [HttpGet("list/{tagName}")]
        public IActionResult GetHeroesByTag(string tagName)
        {
            tagName = tagName.ToLower();
            var tag = _apiContext.Tags.First(x => x.Name.ToLower() == tagName);
            return Ok(_apiContext.Heroes
                .Where(x => x.Tags.Any(t => t.Name == tag.Name))
                .ToList()
                .Select(_mapper.Map<TagJs>)
            );
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

            return Ok(result.ToList().Select(_mapper.Map<HeroJs>));
        }

        [HttpPost("byName")]
        public IActionResult GetHeroByName([FromBody] CamelCaseNameJs camelCaseNameJs)
        {
            var hero = _apiContext.Heroes
                .FirstOrDefault(x => x.Name == camelCaseNameJs.FromCamelCase());

            if (hero is null)
                return NotFound();

            return Ok(_mapper.Map<HeroJs>(hero));
        }


        [HttpGet("{id:int}")]
        public IActionResult GetHeroById(int id)
        {
            var hero = _apiContext.Heroes
                .FirstOrDefault(x => x.Id == id);

            if (hero is null)
                return NotFound();

            return Ok(_mapper.Map<HeroJs>(hero));
        }

        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] HeroJs heroJs)
        {
            var heroInContext = _apiContext.Heroes.Find(heroJs.Id);

            if (heroInContext is null)
                return NotFound();

            heroInContext.Name = heroJs.Name;
            heroInContext.MainAttribute = heroJs.MainAttribute;
            heroInContext.AttackType = heroJs.AttackType;
            var (toDelete, toAdd) = GetListsToModifyHeroTag(
                heroInContext.Tags.Select(x => x.Name).ToList(),
                heroJs.Tags.Select(x => x.Name).ToList());

            foreach (var stringTag in toDelete)
                heroInContext.Tags.Remove(_apiContext.Tags.Find(stringTag));

            foreach (var stringTag in toAdd)
                heroInContext.Tags.Add(_apiContext.Tags.Find(stringTag));

            _apiContext.SaveChanges();

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
            var heroImage = _apiContext.HeroImages.Find(id);
            if (heroImage is null)
                _apiContext.HeroImages.Add(new HeroImage() { Bytes = bytes, HeroId = id });
            else
                heroImage.Bytes = bytes;
            
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
            return Ok(_mapper.Map<HeroJs>(hero));
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromBody] HeroJs heroJs)
        {
            var heroInContext = _apiContext.Heroes.Find(heroJs.Id);
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