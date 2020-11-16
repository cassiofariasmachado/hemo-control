using HemoControl.Entities;
using Microsoft.EntityFrameworkCore;
using HemoControl.Database.Configuration;

namespace HemoControl.Database
{

    public class HemoControlContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Infusion> Infusions { get; set; }

        public HemoControlContext(DbContextOptions<HemoControlContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new InfusionConfiguration());
        }
    }
}