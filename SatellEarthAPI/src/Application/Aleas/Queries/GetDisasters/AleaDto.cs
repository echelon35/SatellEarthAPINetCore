using AutoMapper;
using SatellEarthAPI.Application.Common.Mappings;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Application.Aleas.Queries.GetDisasters;
public class AleaDto : IMapFrom<Alea>
{
    public int Id { get; set; }

    public string Legend { get; set; }

    public string Title { get; set; }

    public IList<DisasterDto> Disasters { get; set; }

    public AleaDto()
    {
        Disasters = new List<DisasterDto>();
    }

}
