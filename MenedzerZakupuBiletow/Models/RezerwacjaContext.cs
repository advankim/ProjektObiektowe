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
        public DbSet<Pasazer> Pasażerowie { get; set; }
        public DbSet<Bilet> Bilety { get; set; }
        public DbSet<Lot> Loty { get; set; }
        public DbSet<Lotnisko> Lotniska { get; set; }
        public DbSet<Samolot> Samoloty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rezerwacja>()
                .HasOne(r => r.Pasazer)
                .WithMany(p => p.Rezerwacje)
                .HasForeignKey(r => r.Id_Pasazer);

            modelBuilder.Entity<Rezerwacja>()
                .HasOne(r => r.Bilet)
                .WithMany()
                .HasForeignKey(r => r.Id_Bilet);

            modelBuilder.Entity<Bilet>()
                .HasOne(b => b.Lot)
                .WithMany()
                .HasForeignKey(b => b.Id_Lot);

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Samolot)
                .WithMany()
                .HasForeignKey(l => l.Id_Samolot);

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.LotniskoWylot)
                .WithMany()
                .HasForeignKey(l => l.Id_Lotnisko_Wylot);

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.LotniskoPrzylot)
                .WithMany()
                .HasForeignKey(l => l.Id_Lotnisko_Przylot);
        }
    }
}