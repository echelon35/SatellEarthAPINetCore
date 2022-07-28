using SatellEarthAPI.Application.Common.Mappings;
using SatellEarthAPI.Domain.Entities;

namespace SatellEarthAPI.Application.TodoItems.Queries.GetTodoItemsWithPagination
{
    public class TodoItemBriefDto : IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public string? Title { get; set; }

        public bool Done { get; set; }
    }
}