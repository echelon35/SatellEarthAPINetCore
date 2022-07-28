using SatellEarthAPI.Application.TodoLists.Queries.ExportTodos;

namespace SatellEarthAPI.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}