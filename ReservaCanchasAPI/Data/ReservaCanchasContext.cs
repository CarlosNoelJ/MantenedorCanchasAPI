using Microsoft.EntityFrameworkCore;
using ReservaCanchasAPI.Models;

namespace ReservaCanchasAPI.Data
{
    public class ReservaCanchasContext : DbContext
    {
        public ReservaCanchasContext(DbContextOptions<ReservaCanchasContext> options) : base(options)
        {       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoCancha>().HasKey(tc => tc.TCanchaId);
            modelBuilder.Entity<Canchas>().HasKey(c => c.CanchaId);

            // Configuración de las relaciones
            modelBuilder.Entity<Canchas>()
                .HasOne(c => c.TipoCancha)
                .WithMany(tc => tc.Canchas)
                .HasForeignKey(c => c.TCanchaId);
        }

        public DbSet<TipoCancha> TipoCancha { get; set; }
        public DbSet<Canchas> Canchas { get; set; }
    }
}
