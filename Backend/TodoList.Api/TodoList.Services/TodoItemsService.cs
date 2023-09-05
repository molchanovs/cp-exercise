using Microsoft.EntityFrameworkCore;
using TodoList.Database;

namespace TodoList.Services
{
    public interface ITodoItemsService
    {
        Task<List<TodoItem>> Get();
    }

    public class TodoItemsService: ITodoItemsService
    {
        private readonly TodoContext _context;

        public TodoItemsService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> Get()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }
    }
}