using Microsoft.EntityFrameworkCore;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Disaster> Disasters { get; }

    DbSet<Alea> Aleas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}