using DotA.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotA.API.Models
{
  public class ApiContext : DbContext
  {
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
      optionsBuilder.UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<HeroImage>().Property(p => p.Bytes)
        .HasColumnType("MediumBlob");
      
      modelBuilder.Entity<Hero>()
        .HasOne(s => s.Image)
        .WithOne(ad => ad.Hero);
      
      modelBuilder.Entity<HeroInMatch>().HasKey(h => new 
      { 
        h.HeroId, 
        h.MatchId 
      });
    }
    

    public DbSet<Hero> Heroes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<HeroImage> HeroImages { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<HeroInMatch> HeroesInMatches { get; set; }
  }
}
