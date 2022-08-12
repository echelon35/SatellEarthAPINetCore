using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using SatellEarthAPI.Domain.Entities;
using SatellEarthAPI.Infrastructure.Converter;

namespace SatellEarthAPI.Infrastructure.Persistence.Configurations;
public class DisasterConfiguration : IEntityTypeConfiguration<Disaster>
{
    /// <summary>
    /// Configure [Disaster] Entity
    /// <param name="builder"></param>
    /// </summary>
    public void Configure(EntityTypeBuilder<Disaster> builder)
    {
        //Autoincrement
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        //Convert NetTopoPoint into custom Point to avoid dependency
        builder.Property(t => t.Point).HasConversion(
            v => GeoJSONToGeomConverter.Convert(v),
            v => JsonConvert.DeserializeObject<Point>(GeoJSONToGeomConverter.ConvertBack(v))
        );

    }
}
