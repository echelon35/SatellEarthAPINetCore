using AutoMapper;
using GeoJSON.Net.Geometry;
using SatellEarthAPI.Application.Common.Mappings;
using SatellEarthAPI.Application.Resolver;

namespace SatellEarthAPI.Application.Aleas.Queries.GetDisasters;
public class PointDto : IMapFrom<Point>
{
    public string Type { get; set; }

    public List<double> Coordinates { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Point, PointDto>()
            .ForMember(d => d.Coordinates, opt => opt.MapFrom(s => new List<double> { s.Coordinates.Longitude, s.Coordinates.Latitude }));
    }

}
