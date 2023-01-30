namespace Dota.Auth.Models
{
    public class Seed
    {
        private readonly AuthContext _context;

        public Seed(AuthContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.Database.EnsureCreated();
        }
    }
}