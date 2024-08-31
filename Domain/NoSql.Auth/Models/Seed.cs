namespace Domain.NoSql.Auth.Models;

public class Seed(AuthContext context)
{
    private readonly AuthContext _context = context;

    public void Create()
    {
        _context.Database.EnsureCreated();
    }
}