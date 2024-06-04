using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Controllers;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Tests.Controllers
{
    public class TodoItemController_Tests
    {
        [Fact]
        public async Task Get_ReturnsOkWithTodoItems()
        {
            // Arrange
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.GetAllTodoItemsAsync())
                .ReturnsAsync(new List<TodoItem>
                {
                new TodoItem { Id = 1, Title = "Task 1", IsCompleted = false },
                new TodoItem { Id = 2, Title = "Task 2", IsCompleted = true }
                });

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var todoItems = okResult.Value.Should().BeAssignableTo<IEnumerable<TodoItem>>().Subject;
            todoItems.Should().HaveCount(2);
        }

        [Fact]
        public async Task Get_ExistingId_ReturnsOkWithTodoItem()
        {
            // Arrange
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.GetTodoItemByIdAsync(1))
                .ReturnsAsync(new TodoItem { Id = 1, Title = "Task 1", IsCompleted = false });

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var todoItem = okResult.Value.Should().BeAssignableTo<TodoItem>().Subject;
            todoItem.Id.Should().Be(1);
            todoItem.Title.Should().Be("Task 1");
        }

        [Fact]
        public async Task Get_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.GetTodoItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((TodoItem)null);

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Post_ValidTodoItem_ReturnsCreatedAtAction()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Task 1", IsCompleted = false };
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.AddTodoItemAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync(1);

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Post(todoItem);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var createdTodoItem = createdAtActionResult.Value.Should().BeAssignableTo<TodoItem>().Subject;
            createdTodoItem.Id.Should().Be(1);
            createdTodoItem.Title.Should().Be("Task 1");
        }

        [Fact]
        public async Task Put_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Task 1", IsCompleted = false };
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.UpdateTodoItemAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync(1);
            mockTodoItemService.Setup(service => service.GetTodoItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(todoItem);

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Put(todoItem);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Put_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var todoItem = new TodoItem { Id = 1, Title = "Task 1", IsCompleted = false };
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.UpdateTodoItemAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync(0);

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Put(todoItem);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.DeleteTodoItemAsync(It.IsAny<int>()))
                .ReturnsAsync(1);
            mockTodoItemService.Setup(service => service.GetTodoItemByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new TodoItem());

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockTodoItemService = new Mock<ITodoItemService>();
            mockTodoItemService.Setup(service => service.DeleteTodoItemAsync(It.IsAny<int>()))
                .ReturnsAsync(0);

            var controller = new TodoItemController(mockTodoItemService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }

}
