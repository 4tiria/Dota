using AutoMapper;
using Domain.NoSql;
using Domain.NoSql.Helpers;
using Dota.API.Helpers;
using Dota.API.Models.EntitiesJs;
using Dota.API.Models.FilterModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using NoSql.Models;

namespace Dota.API.Controllers;

[ApiController, Route("api/match")]
public class MatchController(MongoDbContext context, IMapper mapper) : Controller
{
    private readonly Random _random = new();
    private readonly MongoDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public IActionResult GetAllMatches()
    {
        var matches = _context.Matches.Find(match => true);
        if (!matches.Any())
            return NoContent();

        return Ok(matches
            .ToList()
            .Select(_mapper.Map<MatchJs>)
            .OrderByDescending(x => x.End)
            .ToList());
    }

    [HttpPost("filter")]
    public IActionResult GetBatch([FromBody] MatchFilterModel matchFilterModel)
    {
        var result = ApplyFilters(matchFilterModel);
        if (matchFilterModel.Skip.HasValue)
            result = result.Skip(matchFilterModel.Skip.Value);
        
        if (matchFilterModel.Take.HasValue)
            result = result.Take(matchFilterModel.Take.Value);

        return Ok(result.ToList().Select(_mapper.Map<MatchJs>));
    }

    private IEnumerable<Match> ApplyFilters(MatchFilterModel matchFilterModel)
    {
        var result = _context.Matches.Find(match => true).ToEnumerable();

        if (matchFilterModel.DaysAgo.HasValue)
        {
            var startOfTheDay = DateTime.Today.AddDays(-matchFilterModel.DaysAgo.Value);
            var endOfTheDay = DateTime.Today.AddDays(1 - matchFilterModel.DaysAgo.Value);
            result = result.Where(x => x.Start.Ticks >= startOfTheDay.Ticks && x.Start.Ticks < endOfTheDay.Ticks);
        }

        if (matchFilterModel.MinDurationInMinutes.HasValue)
        {
            var minDurationTicks = matchFilterModel.MinDurationInMinutes.Value * TimeSpan.TicksPerMinute;
            result = result.Where(x => (x.End.Ticks - x.Start.Ticks) >= minDurationTicks);
        }

        if (matchFilterModel.MaxDurationInMinutes.HasValue)
        {
            var minDurationTicks = matchFilterModel.MaxDurationInMinutes.Value * TimeSpan.TicksPerMinute;
            result = result.Where(x => (x.End.Ticks - x.Start.Ticks) <= minDurationTicks);
        }

        if (matchFilterModel.MinStartedMillisecondsBefore.HasValue)
        {
            var minStartTicks = DateTime.Now.Ticks -
                                matchFilterModel.MinStartedMillisecondsBefore.Value * TimeSpan.TicksPerMillisecond;
            result = result.Where(x => x.Start.Ticks >= minStartTicks);
        }

        if (matchFilterModel.MaxStartedMillisecondsBefore.HasValue)
        {
            var maxStartTicks = DateTime.Now.Ticks -
                                matchFilterModel.MaxStartedMillisecondsBefore.Value * TimeSpan.TicksPerMillisecond;
            result = result.Where(x => x.Start.Ticks <= maxStartTicks);
        }

        //todo: add other filters

        return result;
    }

    [HttpPost("addRandom/{count:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddRandomMatches(int count)
    {
        var matches = MockMatchGenerator.CreateMatches(_context, count);
        _context.Matches.InsertMany(matches);
        
        return Ok();
    }
}