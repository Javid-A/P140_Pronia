using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P140_Pronia.Entities;

namespace P140_Pronia.DAL
{
    public class ProniaDbContext : IdentityDbContext<CustomUser>
    {
        public ProniaDbContext(DbContextOptions<ProniaDbContext> options) : base(options)
        {

        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PlantImage> PlantImages { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<PlantCategory> PlantCategories { get; set; }
        public DbSet<PlantInformation> PlantInformations { get; set; }
        public DbSet<CustomUser> CustomUsers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.
                                    GetEntityTypes()
                                    .SelectMany(e => e.GetProperties()
                                                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)
                                                            )
                                                     )
                                    )
            {
                item.SetColumnType("decimal(6,2)");
            }

            //modelBuilder.Entity<Setting>()
            //        .HasIndex(e => e.Key).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
