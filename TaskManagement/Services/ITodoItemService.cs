using TaskManagement.Models;

namespace TaskManagement.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync();
        Task<TodoItem?> GetTodoItemByIdAsync(int id);
        Task<int> AddTodoItemAsync(TodoItem todoItem);
        Task<int> UpdateTodoItemAsync(TodoItem todoItem);
        Task<int> DeleteTodoItemAsync(int id);
    }
}
