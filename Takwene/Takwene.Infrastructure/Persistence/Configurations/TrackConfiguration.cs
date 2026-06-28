using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence.Configurations
{
    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> b)
        {
            b.HasKey(t => t.Id);
            b.Property(t => t.Title).IsRequired().HasMaxLength(300);
            b.Property(t => t.Genre).IsRequired().HasMaxLength(100);

            b.Property(t => t.Isrc).IsRequired().HasMaxLength(12);
            b.HasIndex(t => t.Isrc).IsUnique();

            b.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            b.HasOne(t => t.Artist)
                .WithMany(a => a.Tracks)
                .HasForeignKey(t => t.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
