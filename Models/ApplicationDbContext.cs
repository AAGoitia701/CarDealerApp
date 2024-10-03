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
    }
}
