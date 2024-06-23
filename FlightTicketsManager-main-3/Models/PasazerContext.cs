using Microsoft.EntityFrameworkCore;

namespace ProjektObiektowe.Models
{
    public class PasazerContext : DbContext
    {
        public PasazerContext(DbContextOptions<PasazerContext> options)
           : base(options)
        {
        }

        public DbSet<Pasazer> Pasazerowie { get; set; }
    }
}
