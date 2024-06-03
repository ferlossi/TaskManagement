using FluentAssertions;
using Moq;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Tests.Services
{
    public class TodoItemService_Tests
    {
        private readonly Mock<IRepository<TodoItem>> _mockTodoItemRepository;
        private readonly TodoItemService _todoItemService;

        public TodoItemService_Tests()
        {
            _mockTodoItemRepository = new Mock<IRepository<TodoItem>>();
            _todoItemService = new TodoItemService(_mockTodoItemRepository.Object);
        }

        [Fact]
        public async Task GetAllTodoItemsAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Description = "Task 1", IsCompleted = false },
                new TodoItem { Id = 2, Description = "Task 2", IsCompleted = true }
            };
            _mockTodoItemRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(todoItems);

            // Act
            var result = await _todoItemService.GetAllTodoItemsAsync();

            // Assert
            result.Should().BeEquivalentTo(todoItems);
        }

        [Fact]
        public async Task GetTodoItemByIdAsync_ShouldReturnTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Description = "Task 1", IsCompleted = false };
            _mockTodoItemRepository.Setup(repo => repo.GetByIdAsync(todoItem.Id)).ReturnsAsync(todoItem);

            // Act
            var result = await _todoItemService.GetTodoItemByIdAsync(todoItem.Id);

            // Assert
            result.Should().BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task AddTodoItemAsync_ShouldAddTask()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Description = "Task 1", IsCompleted = false };
            _mockTodoItemRepository.Setup(repo => repo.AddAsync(todoItem)).ReturnsAsync(1);

            // Act
            var result = await _todoItemService.AddTodoItemAsync(todoItem);

            // Assert
            result.Should().Be(1);
            _mockTodoItemRepository.Verify(repo => repo.AddAsync(todoItem), Times.Once);
        }

        [Fact]
        public async Task UpdateTodoItemAsync_ShouldUpdateTask()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Description = "Task 1", IsCompleted = false };
            _mockTodoItemRepository.Setup(repo => repo.UpdateAsync(todoItem)).ReturnsAsync(1);

            // Act
            var result = await _todoItemService.UpdateTodoItemAsync(todoItem);

            // Assert
            result.Should().Be(1);
            _mockTodoItemRepository.Verify(repo => repo.UpdateAsync(todoItem), Times.Once);
        }

        [Fact]
        public async Task DeleteTodoItemAsync_ShouldDeleteTask()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Description = "Task 1", IsCompleted = false };
            _mockTodoItemRepository.Setup(repo => repo.DeleteAsync(todoItem.Id)).ReturnsAsync(1);

            // Act
            var result = await _todoItemService.DeleteTodoItemAsync(todoItem.Id);

            // Assert
            result.Should().Be(1);
            _mockTodoItemRepository.Verify(repo => repo.DeleteAsync(todoItem.Id), Times.Once);
        }
    }

}
