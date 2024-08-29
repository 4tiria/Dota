using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Dota.Common;

namespace DataAccess.Seeds
{
    public class HeroSeed : ISeed
    {
        private readonly ApiContext _context;
        private readonly Random _random = new(42);

        public HeroSeed(ApiContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
           //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            if (!_context.Tags.Any())
            {
                var tags = new List<string>()
                        { "Carry", "Support", "Pusher", "Nuker", "Escape", "Durable", "Disabler", "Initiator" }
                    .OrderBy(x => x)
                    .Select(x => new Tag() { Name = x }).ToList();

                foreach (var tag in tags)
                    _context.Tags.Add(tag);

                _context.SaveChanges();
            }

            if (!_context.Heroes.Any())
            {
                var heroes = new List<Hero>()
                {
                    new Hero()
                    {
                        Name = "Abaddon", MainAttribute = "Strength", AttackType = "Melee", Tags = new List<string>()
                                { "Support", "Carry", "Durable" }.Select(x => _context.Tags.First(f => f.Name == x))
                            .ToList()
                    },

                    new Hero()
                    {
                        Name = "Earthshaker", MainAttribute = "Strength", AttackType = "Melee", Tags =
                            new List<string>()
                                    { "Support", "Initiator", "Disabler", "Nuker" }
                                .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Hoodwink", MainAttribute = "Agility", AttackType = "Range", Tags = new List<string>()
                                { "Support", "Nuker", "Escape", "Disabler", }
                            .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Invoker", MainAttribute = "Intelligence", AttackType = "Range", Tags =
                            new List<string>()
                                    { "Carry", "Nuker", "Disabler", "Escape", "Pusher" }
                                .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Naga Siren", MainAttribute = "Agility", AttackType = "Melee", Tags = new List<string>()
                                { "Carry", "Support", "Pusher", "Disabler", "Initiator", "Escape" }
                            .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Shadow Demon", MainAttribute = "Intelligence", AttackType = "Range", Tags =
                            new List<string>()
                                    { "Support", "Disabler", "Initiator", "Nuker" }
                                .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Slardar", MainAttribute = "Strength", AttackType = "Melee", Tags = new List<string>()
                                { "Carry", "Durable", "Initiator", "Disabler", "Escape" }
                            .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Snapfire", MainAttribute = "Strength", AttackType = "Range", Tags = new List<string>()
                                { "Support", "Nuker", "Disabler", "Escape" }
                            .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Sven", MainAttribute = "Strength", AttackType = "Melee", Tags = new List<string>()
                                { "Carry", "Disabler", "Initiator", "Durable", "Nuker" }
                            .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Timbersaw", MainAttribute = "Strength", AttackType = "Melee", Tags = new List<string>()
                                { "Nuker", "Durable", "Escape" }
                            .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },

                    new Hero()
                    {
                        Name = "Vengeful Spirit", MainAttribute = "Agility", AttackType = "Range", Tags =
                            new List<string>()
                                    { "Support", "Initiator", "Disabler", "Nuker", "Escape" }
                                .Select(x => _context.Tags.First(f => f.Name == x)).ToList()
                    },
                };

                foreach (var hero in heroes)
                    _context.Heroes.Add(hero);

                _context.SaveChanges();
            }
        }
    }
}