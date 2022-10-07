using System.Collections.Generic;
using System.Linq;

namespace DotA.API.Models
{
  public class HeroSeed
  {
    private readonly ApiContext _context;

    public HeroSeed(ApiContext context)
    {
      _context = context;
    }

    public void SeedData()
    {
      _context.Database.EnsureDeleted();
      _context.Database.EnsureCreated();

      if (!_context.Tags.Any())
      {
        var tags = new List<string>()
            { "Carry", "Support", "Pusher", "Nuker", "Escape" }
          .Select(x => new Tag() { Name = x }).ToList();

        foreach (var tag in tags)
          _context.Tags.Add(tag);

        _context.SaveChanges();
      }

      if (!_context.Heroes.Any())
      {
        var sd = new Hero() { Name = "Shadow Demon", MainAttribute = "Intelligence" };
        sd.Tags.Add(_context.Tags.First(x => x.Name == "Support"));
        sd.Tags.Add(_context.Tags.First(x => x.Name == "Nuker"));

        var heroes = new List<Hero>()
        {
          new Hero() { Name = "Sven", MainAttribute = "Strength", Tags = new List<string>()
                { "Carry" }.Select(x => _context.Tags.First(f => f.Name == x)).ToList()},

          new Hero() { Name = "Naga Siren", MainAttribute = "Agility", Tags = new List<string>()
            { "Carry", "Pusher", "Escape" }.Select(x => _context.Tags.First(f => f.Name == x)).ToList() },

          new Hero() { Name = "Invoker", MainAttribute = "Intelligence", Tags = new List<string>()
            { "Carry", "Nuker", "Escape" }.Select(x => _context.Tags.First(f => f.Name == x)).ToList() },
        };

        heroes.Add(sd);

        foreach (var hero in heroes)
          _context.Heroes.Add(hero);

        _context.SaveChanges();
      }
    }
  }
}
