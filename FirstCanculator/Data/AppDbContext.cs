using FirstCanculator.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstCanculator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<CanculatorModels> Canculators { get; set; }
    }
}
