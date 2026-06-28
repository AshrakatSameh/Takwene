using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Takwene.Domain.Models;

namespace Takwene.Infrastructure.Persistence.Configurations
{
    public class DspConfiguration : IEntityTypeConfiguration<Dsp>
    {
        public void Configure(EntityTypeBuilder<Dsp> b)
        {
            b.HasKey(d => d.Id);
            b.Property(d => d.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(d => d.Name).IsUnique();
        }
    }
}
