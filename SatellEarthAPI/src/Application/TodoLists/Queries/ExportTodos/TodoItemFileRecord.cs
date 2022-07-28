using SatellEarthAPI.Application.Common.Mappings;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string? Title { get; set; }

        public bool Done { get; set; }
    }
}