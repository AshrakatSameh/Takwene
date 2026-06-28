using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence.Configurations
{
    public class TrackDistributionConfiguration : IEntityTypeConfiguration<TrackDistribution>
    {
        public void Configure(EntityTypeBuilder<TrackDistribution> b)
        {
            b.HasKey(d => d.Id);
            b.Property(d => d.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            b.HasOne(d => d.Track)
                .WithMany(t => t.Distributions)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(d => d.Dsp)
                .WithMany(x => x.Distributions)
                .HasForeignKey(d => d.DspId)
                .OnDelete(DeleteBehavior.Restrict);

            // A track cannot be distributed to the same DSP more than once.
            b.HasIndex(d => new { d.TrackId, d.DspId }).IsUnique();
        }
    }
}
