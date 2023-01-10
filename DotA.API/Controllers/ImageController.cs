using DotA.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using DotA.API.Models.Entities;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/image")]
    public class ImageController : Controller
    {
        //todo: delete controller
        private readonly ApiContext _apiContext;
        
        public ImageController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult GetAllHeroImages()
        {
            var images = _apiContext.HeroImages.ToList();
            if (!images.Any())
                return NoContent();
            
            return Ok(images);
        }

        [HttpPost("guid")]
        public IActionResult GetImageByGuid([FromBody] string guidString)
        {
            if (!Guid.TryParse(guidString, out var guid))
            {
                return BadRequest();
            }

            var image = _apiContext.HeroImages.Find(guid);
            if (image is null)
            {
                return NotFound();
            }

            return Ok(image);
        }
    }
}