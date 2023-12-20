using Microsoft.EntityFrameworkCore;
using P140_Pronia.Entities;

namespace P140_Pronia.DAL
{
    public class ProniaDbContext : DbContext
    {
        public ProniaDbContext(DbContextOptions<ProniaDbContext> options) : base(options)
        {

        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
    }
}
