using Microsoft.EntityFrameworkCore;
using BonsaiAPI.Models;

namespace BonsaiAPI.Data
{
    public class BonsaiContext : DbContext
    {
        public BonsaiContext(DbContextOptions<BonsaiContext> options) : base(options)
        {
        }

        public DbSet<Species> Species { get; set; } = default!;
        public DbSet<Tree> Trees { get; set; } = default!;
    }
}