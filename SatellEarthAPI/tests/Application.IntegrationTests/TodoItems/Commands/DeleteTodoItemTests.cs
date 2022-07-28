using FluentAssertions;
using NUnit.Framework;
using SatellEarthAPI.Application.Common.Exceptions;
using SatellEarthAPI.Application.TodoItems.Commands.CreateTodoItem;
using SatellEarthAPI.Application.TodoItems.Commands.DeleteTodoItem;
using SatellEarthAPI.Application.TodoLists.Commands.CreateTodoList;
using SatellEarthAPI.Domain.Entities;
using static SatellEarthAPI.Application.IntegrationTests.Testing;

namespace SatellEarthAPI.Application.IntegrationTests.TodoItems.Commands
{
    public class DeleteTodoItemTests : BaseTestFixture
    {
        [Test]
        public async Task ShouldRequireValidTodoItemId()
        {
            var command = new DeleteTodoItemCommand(99);

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteTodoItem()
        {
            var listId = await SendAsync(new CreateTodoListCommand
            {
                Title = "New List"
            });

            var itemId = await SendAsync(new CreateTodoItemCommand
            {
                ListId = listId,
                Title = "New Item"
            });

            await SendAsync(new DeleteTodoItemCommand(itemId));

            var item = await FindAsync<TodoItem>(itemId);

            item.Should().BeNull();
        }
    }
}