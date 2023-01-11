using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess;
using DataAccess.Models;
using DotA.API.Models;
using DotA.API.Models.EntitiesJs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/tag")]
    public class TagController : Controller
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;

        public TagController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_apiContext.Tags.ToList().Select(_mapper.Map<TagJs>));
        }
    }
}