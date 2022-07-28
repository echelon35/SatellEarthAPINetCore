using FluentAssertions;
using NUnit.Framework;
using SatellEarthAPI.Application.Common.Exceptions;
using SatellEarthAPI.Application.TodoLists.Commands.CreateTodoList;
using SatellEarthAPI.Application.TodoLists.Commands.DeleteTodoList;
using SatellEarthAPI.Domain.Entities;
using static SatellEarthAPI.Application.IntegrationTests.Testing;

namespace SatellEarthAPI.Application.IntegrationTests.TodoLists.Commands
{
    public class DeleteTodoListTests : BaseTestFixture
    {
        [Test]
        public async Task ShouldRequireValidTodoListId()
        {
            var command = new DeleteTodoListCommand(99);
            await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoList()
        {
            var listId = await SendAsync(new CreateTodoListCommand
            {
                Title = "New List"
            });

            await SendAsync(new DeleteTodoListCommand(listId));

            var list = await FindAsync<TodoList>(listId);

            list.Should().BeNull();
        }
    }
}