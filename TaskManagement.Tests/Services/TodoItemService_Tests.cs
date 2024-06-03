using FluentAssertions;
using Moq;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Tests.Services
{
    public class TodoItemService_Tests
    {
        [Fact]
        public async Task GetTodoItemByIdAsync_ExistingId_ReturnsTodoItem()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<TodoItem>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new TodoItem { Id = 1, Title = "Test Todo", IsCompleted = false });
            var todoItemService = new TodoItemService(mockRepository.Object);

            // Act
            var todoItem = await todoItemService.GetTodoItemByIdAsync(1);

            // Assert
            todoItem.Should().NotBeNull();
            todoItem?.Title.Should().Be("Test Todo");
        }

        [Fact]
        public async Task GetTodoItemByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<TodoItem>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((TodoItem)null);
            var todoItemService = new TodoItemService(mockRepository.Object);

            // Act
            var todoItem = await todoItemService.GetTodoItemByIdAsync(999);

            // Assert
            todoItem.Should().BeNull();
        }
    }

}
