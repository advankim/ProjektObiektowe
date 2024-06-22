using Microsoft.EntityFrameworkCore;

namespace ProjektObiektowe.Models
{
    public class LotContext : DbContext
    {
        public LotContext(DbContextOptions<LotContext> options)
            : base(options)
        {
        }

        public DbSet<Samolot> Samoloty { get; set; }
        public DbSet<Lotnisko> Lotniska { get; set; }
        public DbSet<Lot> Loty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Samolot)
                .WithMany()
                .HasForeignKey(l => l.Id_Samolot)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Lotnisko_Wylot)
                .WithMany()
                .HasForeignKey(l => l.Id_Lotnisko_Wylot)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Lotnisko_Przylot)
                .WithMany()
                .HasForeignKey(l => l.Id_Lotnisko_Przylot)
                .OnDelete(DeleteBehavior.Restrict); 

          

            base.OnModelCreating(modelBuilder);
        }



    }
}
