using CarDealerApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CarDealerApp.Models
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Car> Cars {  get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar la relación uno a muchos (opcional) entre Car y Owner
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Owner)
                .WithMany(o => o.ListCars)
                .HasForeignKey(c => c.OwnerId)
                .IsRequired(false);  // optional relation

            base.OnModelCreating(modelBuilder);
        }
    }
}
