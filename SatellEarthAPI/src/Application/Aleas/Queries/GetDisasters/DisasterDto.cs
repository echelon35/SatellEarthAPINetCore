using AutoMapper;
using SatellEarthAPI.Application.Common.Mappings;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Application.Aleas.Queries.GetDisasters;

/// <summary>
/// Modelisation of [Disaster] inside [Alea]
/// </summary>
public class DisasterDto : IMapFrom<Disaster>
{
    public int Id { get; set; }

    public int AleaId { get; set; }

    public string? LienSource { get; set; }

    public DateTime PremierReleve { get; set; }

    public DateTime DernierReleve { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Disaster, DisasterDto>();
    }
}
