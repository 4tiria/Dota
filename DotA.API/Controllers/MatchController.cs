using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess;
using DataAccess.Models;
using DotA.API.Helpers;
using DotA.API.Models;
using DotA.API.Models.EntitiesJs;
using DotA.API.Models.FilterModels;
using Microsoft.AspNetCore.Mvc;

namespace DotA.API.Controllers
{
    [ApiController, Route("api/match")]
    public class MatchController : Controller
    {
        private readonly Random _random = new Random();
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public MatchController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllMatches()
        {
            var matches = _context.Matches;
            if (!matches.Any())
                return NoContent();

            return Ok(matches
                .ToList()
                .Select(x => { return _mapper.Map<MatchJs>(x); })
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
            var result = _context.Matches.AsEnumerable();
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
        public IActionResult AddRandomMatches(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var guid = Guid.NewGuid();
                var randomDuration = _random.Next(1000 * 60 * 15, 1000 * 60 * 60);
                var randomDateStart = DateTime.Now.AddHours(_random.NextDouble() * 8 - 4);
                var randomDateEnd = randomDateStart.AddMilliseconds(randomDuration);
                var radiantScore = _random.Next(1, 40);
                var direScore = _random.Next(1, 40);
                var winner = (_random.NextDouble() * radiantScore / direScore) > 0.5 ? "Radiant" : "Dire";
                var match = new Match()
                {
                    Id = guid,
                    Start = randomDateStart,
                    End = randomDateEnd,
                    Heroes = GenerateHeroesInMatch(guid, radiantScore, direScore),
                    Score = $"{radiantScore}-{direScore}",
                    WinnerSide = winner,
                };

                _context.Matches.Add(match);
                _context.SaveChanges();
            }

            return Ok();
        }

        private List<HeroInMatch> GenerateHeroesInMatch(Guid guid, int radiantScore, int direScore)
        {
            var heroPull = _context.Heroes.ToList().Sample(10);
            var radiant = heroPull.Sample(5).Shuffle();
            var dire = heroPull.Except(radiant).ToList().Shuffle();

            var radiantKills = radiantScore.SplitNumber(radiant.Count, 0);
            var radiantDeaths = direScore.SplitNumber(dire.Count, 0);
            var direKills = direScore.SplitNumber(dire.Count, 0);
            var direDeaths = radiantScore.SplitNumber(radiant.Count, 0);

            var radiantTeam = GenerateHeroStatistics(guid, "radiant", radiant,
                radiantKills, radiantDeaths, radiantScore);
            var direTeam = GenerateHeroStatistics(guid, "dire", dire,
                direKills, direDeaths, direScore);

            return radiantTeam.Concat(direTeam).ToList();
        }

        private List<HeroInMatch> GenerateHeroStatistics(Guid guid,
            string side, List<Hero> heroes, List<int> kills, List<int> deaths, int score)
        {
            var result = new List<HeroInMatch>();
            for (var i = 0; i < heroes.Count; i++)
            {
                var currentHero = new HeroInMatch()
                {
                    Hero = heroes[i],
                    HeroId = heroes[i].Id,
                    MatchId = guid,
                    Kills = kills[i],
                    Deaths = deaths[i],
                    Assists = _random.Next(score / 2, score),
                    Gold = (int)((_random.NextDouble() * 10000 + 5000) * Math.Sqrt(kills[i] / (double)score)),
                    XP = (int)((_random.NextDouble() * 8000 + 7000) * Math.Sqrt(kills[i] / (double)score)),
                    Side = side,
                };

                result.Add(currentHero);
            }

            return result;
        }
    }
}