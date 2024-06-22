using Microsoft.EntityFrameworkCore;

namespace MenedzerZakupuBiletow.Models
{
    public class RezerwacjaContext : DbContext
    {
        public RezerwacjaContext(DbContextOptions<RezerwacjaContext> options)
           : base(options)
        {
        }

        public DbSet<Rezerwacja> Rezerwacje { get; set; }
        
        /*public DbSet<Lot> Loty { get; set; }
        public DbSet<Samolot> Samoloty { get; set; }
        public DbSet<Lotnisko> Lotniska { get; set; }*/

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rezerwacja>()
                .HasOne(b => b.Bilety)
                .WithMany(l => l.Bilety)
                .HasForeignKey(b => b.Id_Pasazer)
                .HasForeignKey(b => b.Id_Bilet)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }*/
    }
}
