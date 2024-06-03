using TaskManagement.Data;
using TaskManagement.Models;

namespace TaskManagement.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly IRepository<TodoItem> _todoItemRepository;

        public TodoItemService(IRepository<TodoItem> todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public Task<IEnumerable<TodoItem>> GetAllTodoItemsAsync()
        {
            return _todoItemRepository.GetAllAsync();
        }

        public Task<TodoItem?> GetTodoItemByIdAsync(int id)
        {
            return _todoItemRepository.GetByIdAsync(id);
        }

        public Task<int> AddTodoItemAsync(TodoItem todoItem)
        {
            return _todoItemRepository.AddAsync(todoItem);
        }

        public Task<int> UpdateTodoItemAsync(TodoItem todoItem)
        {
            return _todoItemRepository.UpdateAsync(todoItem);
        }

        public Task<int> DeleteTodoItemAsync(int id)
        {
            return _todoItemRepository.DeleteAsync(id);
        }
    }

}
