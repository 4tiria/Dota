using Domain.NoSql.Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.NoSql.Auth.Models;

public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
            .ToTable("account")
            .Property(c => c.AccessLevel)
            .HasConversion<string>();

        modelBuilder.Entity<RefreshToken>()
            .ToTable("refreshtoken");
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}