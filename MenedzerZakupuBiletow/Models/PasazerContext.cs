using Microsoft.EntityFrameworkCore;

namespace MenedzerZakupuBiletow.Models
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
