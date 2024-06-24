using Microsoft.EntityFrameworkCore;

namespace ProjektObiektowe.Models
{
    public class SamolotContext : DbContext
    {
        public SamolotContext(DbContextOptions<SamolotContext> options)
           : base(options)
        {
        }

        public DbSet<Samolot> Samoloty { get; set; }
        public DbSet<Lot> Loty { get; set; }


    }
}
