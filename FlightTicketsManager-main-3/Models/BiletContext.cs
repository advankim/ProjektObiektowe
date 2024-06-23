using Microsoft.EntityFrameworkCore;
using ProjektObiektowe.Models;

public class BiletContext : DbContext
{
    public BiletContext(DbContextOptions<BiletContext> options)
        : base(options)
    {
    }

    public DbSet<Bilet> Bilety { get; set; }
    public DbSet<Lot> Loty { get; set; }
    public DbSet<Samolot> Samoloty { get; set; }
    public DbSet<Lotnisko> Lotniska { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bilet>()
            .HasOne(b => b.Lot)
            .WithMany(l => l.Bilety)
            .HasForeignKey(b => b.Id_Lot)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}