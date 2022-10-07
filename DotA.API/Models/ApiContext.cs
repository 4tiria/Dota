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

      // modelBuilder.Entity<Tag>()
      //   .HasOne(r => r.Hero)
      //   .WithMany(b => b.Tags)
      //   .OnDelete(DeleteBehavior.Cascade);

    }

    public DbSet<Hero> Heroes { get; set; }
    public DbSet<Tag> Tags { get; set; }
  }
}
