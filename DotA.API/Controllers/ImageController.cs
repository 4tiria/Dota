using DotA.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using AutoMapper;
using DataAccess;
using DotA.API.Models.EntitiesJs;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/image")]
    public class ImageController : Controller
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;
        
        public ImageController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllHeroImages()
        {
            var images = _apiContext.HeroImages.ToList();
            if (!images.Any())
                return NoContent();
            
            return Ok(images.ToList().Select(_mapper.Map<HeroImageJs>));
        }

        [HttpPost("guid")]
        public IActionResult GetImageByGuid([FromBody] string guidString)
        {
            if (!Guid.TryParse(guidString, out var guid))
                return BadRequest();

            var image = _apiContext.HeroImages.Find(guid);
            if (image is null)
                return NotFound();

            return Ok(_mapper.Map<HeroImageJs>(image));
        }
    }
}