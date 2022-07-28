using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Infrastructure.Persistence.Configurations;
public class DisasterConfiguration : IEntityTypeConfiguration<Disaster>
{
    public void Configure(EntityTypeBuilder<Disaster> builder)
    {
        builder.Property(t => t.LienSource)
            .HasMaxLength(50)
            .IsRequired();
    }
}
