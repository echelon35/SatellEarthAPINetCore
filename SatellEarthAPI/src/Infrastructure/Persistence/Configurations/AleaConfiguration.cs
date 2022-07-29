using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Infrastructure.Persistence.Configurations;

public class AleaConfiguration : IEntityTypeConfiguration<Alea>
{
    public void Configure(EntityTypeBuilder<Alea> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Legend)
            .HasMaxLength(50)
            .IsRequired();
    }
}
