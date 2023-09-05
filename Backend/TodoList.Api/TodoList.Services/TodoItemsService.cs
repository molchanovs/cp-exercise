using Microsoft.EntityFrameworkCore;
using TodoList.Database;
using TodoList.Models;

namespace TodoList.Services
{
    public interface ITodoItemsService
    {
        Task<List<TodoItem>> GetAll(bool completed = false);
        Task<TodoItem> Get(Guid id);
        Task Update(Guid id, TodoItem item);
        Task Add(TodoItem item);
    }

    public class TodoItemsService: ITodoItemsService
    {
        private readonly TodoContext _context;

        public TodoItemsService(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetAll(bool completed = false)
        {
            return await _context.TodoItems.Where(x => x.IsCompleted == completed).ToListAsync();
        }

        public async Task<TodoItem> Get(Guid id)
        {
            var result = await _context.TodoItems.FindAsync(id);
            if (result == null) throw new NotFoundException();
            return result;
        }

        public async Task Update(Guid id, TodoItem item)
        {
            if (id != item.Id)
            {
                throw new ApiException();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TodoItemIdExists(id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Add(TodoItem item)
        {
            if (string.IsNullOrEmpty(item?.Description))
            {
                throw new ApiException("Description is required");
            }
            else if (await TodoItemDescriptionExists(item.Description))
            {
                throw new ApiException("Description already exists");
            }

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> TodoItemIdExists(Guid id)
        {
            return await _context.TodoItems.AnyAsync(x => x.Id == id);
        }

        private async Task<bool> TodoItemDescriptionExists(string description, bool isCompleted = false)
        {
            return await _context.TodoItems.
                AnyAsync(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && x.IsCompleted == isCompleted);
        }
    }
}