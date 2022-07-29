using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SatellEarthAPI.Application.Common.Interfaces;
using SatellEarthAPI.Application.Common.Security;

namespace SatellEarthAPI.Application.Aleas.Queries.GetDisasters;

[Authorize]
public record GetDisastersQuery : IRequest<DisastersVm>;

public class GetDisastersQueryHandler : IRequestHandler<GetDisastersQuery, DisastersVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDisastersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DisastersVm> Handle(GetDisastersQuery request, CancellationToken cancellationToken)
    {
        return new DisastersVm
        {
            Lists = await _context.Aleas
                .AsNoTracking()
                .ProjectTo<AleaDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Legend)
                .ToListAsync(cancellationToken)
        };
    }
}
