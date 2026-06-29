using Microsoft.EntityFrameworkCore;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Artist> Artists => Set<Artist>();
        public DbSet<Track> Tracks => Set<Track>();
        public DbSet<Dsp> Dsps => Set<Dsp>();
        public DbSet<TrackDistribution> TrackDistributions => Set<TrackDistribution>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all IEntityTypeConfiguration<T> classes in this assembly.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Seed data
            SeedData.Apply(modelBuilder);
        }
    }
}
