using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence.Configurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> b)
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.Name).IsRequired().HasMaxLength(200);
            b.Property(a => a.Email).IsRequired().HasMaxLength(256);
            b.Property(a => a.Country).IsRequired().HasMaxLength(100);
        }
    }
}
