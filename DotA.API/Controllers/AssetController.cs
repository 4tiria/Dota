using System.Linq;
using AutoMapper;
using DataAccess;
using DataAccess.Models;
using DotA.API.Helpers;
using DotA.API.Models.EntitiesJs;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/asset")]
    public class AssetController : Controller
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public AssetController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var asset = _context.Assets.Find(name);
            if (asset is null)
                return NotFound();

            return Ok(_mapper.Map<AssetJs>(asset));
        }

        [HttpGet]
        public IActionResult GetAllAssets()
        {
            if (!_context.Assets.Any())
                return NoContent();

            return Ok(_context.Assets.ToList().Select(_mapper.Map<AssetJs>));
        }

        [HttpPost]
        public IActionResult PostAsset()
        {
            var files = Request.Form.Files;
            if (!files.Any())
                return BadRequest();
            var blob = files[0].ToByteArray();
            var name = files[0].FileName;

            if (_context.Assets.Find(name)!=null)
            {
                return BadRequest($"{name}: file already exists");
            }
            
            var asset = new Asset() { Blob = blob, Name = name };
            _context.Assets.Add(asset);
            _context.SaveChanges();
            return Ok();
        }
    }
}