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
        
        [HttpPost("file")]
        public IActionResult PostTestFile()
        {
            var file = Request.Form.Files[0];
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            _apiContext.HeroImages.Add(new HeroImage() { Bytes = fileBytes});
            _apiContext.SaveChanges();

            return Ok();
        }

        [HttpGet("file")]
        public IActionResult GetTestImage()
        {
            if (!_apiContext.HeroImages.Any())
            {
                return NotFound();
            }
            var last = _apiContext.HeroImages.First();
            return Ok(last);
        }

        [HttpPost("base64")]
        public void TestBlob([FromBody] string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            _apiContext.HeroImages.Add(new HeroImage(){Bytes = data});
            //_apiContext.SaveChanges();
            using(var stream = new MemoryStream(data, 0, data.Length))
            {
                var image = Image.FromStream(stream);
                image.Save(@"D:\image.png");
            }
        }
    }
}