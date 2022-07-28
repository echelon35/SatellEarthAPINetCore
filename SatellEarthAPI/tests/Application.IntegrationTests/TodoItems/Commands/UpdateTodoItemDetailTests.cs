using FluentAssertions;
using NUnit.Framework;
using SatellEarthAPI.Application.Common.Exceptions;
using SatellEarthAPI.Application.TodoItems.Commands.CreateTodoItem;
using SatellEarthAPI.Application.TodoItems.Commands.UpdateTodoItem;
using SatellEarthAPI.Application.TodoItems.Commands.UpdateTodoItemDetail;
using SatellEarthAPI.Application.TodoLists.Commands.CreateTodoList;
using SatellEarthAPI.Domain.Entities;
using SatellEarthAPI.Domain.Enums;
using static SatellEarthAPI.Application.IntegrationTests.Testing;

namespace SatellEarthAPI.Application.IntegrationTests.TodoItems.Commands
{
    public class UpdateTodoItemDetailTests : BaseTestFixture
    {
        [Test]
        public async Task ShouldRequireValidTodoItemId()
        {
            var command = new UpdateTodoItemCommand { Id = 99, Title = "New Title" };
            await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Test]
        public async Task ShouldUpdateTodoItem()
        {
            var userId = await RunAsDefaultUserAsync();

            var listId = await SendAsync(new CreateTodoListCommand
            {
                Title = "New List"
            });

            var itemId = await SendAsync(new CreateTodoItemCommand
            {
                ListId = listId,
                Title = "New Item"
            });

            var command = new UpdateTodoItemDetailCommand
            {
                Id = itemId,
                ListId = listId,
                Note = "This is the note.",
                Priority = PriorityLevel.High
            };

            await SendAsync(command);

            var item = await FindAsync<TodoItem>(itemId);

            item.Should().NotBeNull();
            item!.ListId.Should().Be(command.ListId);
            item.Note.Should().Be(command.Note);
            item.Priority.Should().Be(command.Priority);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        }
    }
}