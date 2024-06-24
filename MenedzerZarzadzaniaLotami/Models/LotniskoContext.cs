using Microsoft.EntityFrameworkCore;

namespace ProjektObiektowe.Models
{
    public class LotniskoContext : DbContext
    {
        public LotniskoContext(DbContextOptions<LotniskoContext> options)
            : base(options)
        {
        }

        public DbSet<Lotnisko> Lotniska { get; set; }
        public DbSet<Lot> Loty { get; set; }

    }
}
