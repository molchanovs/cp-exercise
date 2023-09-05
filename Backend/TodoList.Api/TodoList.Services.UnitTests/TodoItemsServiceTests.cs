using Microsoft.EntityFrameworkCore;
using TodoList.Database;
using TodoList.Models;
using Xunit;

namespace TodoList.Services.UnitTests
{
    public class TodoItemsServiceTests
    {

        [Fact]
        public async Task WhenAddTodoItemWithEmptyDescription_ThrowsException()
        {
            var context = GetTodoContext();
            var service = new TodoItemsService(context);
            Func<Task> act = async () => await service.Add(new TodoItem());
            var exception = await Assert.ThrowsAsync<ApiException>(act);
            Assert.Equal("Description is required", exception.Message);
        }

        [Fact]
        public async Task WhenAddTodoItemWithExistingDescription_ThrowsException()
        {
            var context = GetTodoContext();
            var service = new TodoItemsService(context);
            var item = new TodoItem { Description = "blah" };
            await service.Add(item);
            Func<Task> act = async () => await service.Add(item);
            var exception = await Assert.ThrowsAsync<ApiException>(act);
            Assert.Equal("Description already exists", exception.Message);
        }

        [Fact]
        public async Task WhenUpdateTodoItemWithInvalidId_ThrowsException()
        {
            var context = GetTodoContext();
            var service = new TodoItemsService(context);
            Func<Task> act = async () => await service.Update(Guid.NewGuid(), new TodoItem());
            await Assert.ThrowsAsync<ApiException>(act);
        }

        [Fact]
        public async Task WhenUpdateTodoItemAndIdNotFound_ThrowsException()
        {
            var context = GetTodoContext();
            var service = new TodoItemsService(context);
            var id = Guid.NewGuid();
            Func<Task> act = async () => await service.Update(id, new TodoItem { Id = id });
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        public TodoContext GetTodoContext()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TodoListDatabase")
                .Options;

            return new TodoContext(options);
        }
    }
}