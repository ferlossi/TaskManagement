using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Controllers;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Tests.Controllers
{
    public class UserController_Tests
    {
        [Fact]
        public async Task Get_ReturnsOkWithUsers()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetAllUsersAsync())
                .ReturnsAsync(new List<User>
                {
                new User { Id = 1, Username = "User1" , Password="Password 1"},
                new User { Id = 2, Username = "User2" , Password="Password 2"}
                });

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var users = okResult.Value.Should().BeAssignableTo<IEnumerable<User>>().Subject;
            users.Should().HaveCount(2);
        }

        [Fact]
        public async Task Get_ExistingId_ReturnsOkWithUser()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, Username = "User1" , Password="Password 1"});

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var user = okResult.Value.Should().BeAssignableTo<User>().Subject;
            user.Id.Should().Be(1);
            user.Username.Should().Be("User1");
        }

        [Fact]
        public async Task Get_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Post_ValidUser_ReturnsCreatedAtAction()
        {
            // Arrange
            var user = new User { Id = 1, Username = "User1", Password = "Password" };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(user.Id);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Post(user);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be(nameof(UserController.Get));
            createdAtActionResult.RouteValues["id"].Should().Be(user.Id);
            var createdUser = createdAtActionResult.Value.Should().BeAssignableTo<User>().Subject;
            createdUser.Username.Should().Be("User1");
        }

        [Fact]
        public async Task Put_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            var user = new User { Id = 1, Username = "User1", Password = "Password" };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync(user);
            mockUserService.Setup(service => service.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(1);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Put(user);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Put_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var user = new User { Id = 1, Username = "User1", Password = "Password" };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync((User)null);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Put(user);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_ExistingUser_ReturnsNoContent()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync(new User { Id = 1, Username = "User1" , Password="Password 1"});
            mockUserService.Setup(service => service.DeleteUserAsync(1))
                .ReturnsAsync(1);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(1))
                .ReturnsAsync((User)null);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsOkWithUser()
        {
            // Arrange
            var user = new User { Id = 1, Username = "User1", Password = "Password" };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.AuthenticateUserAsync(user.Username, user.Password))
                .ReturnsAsync(user);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Authenticate(user);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var authenticatedUser = okResult.Value.Should().BeAssignableTo<User>().Subject;
            authenticatedUser.Username.Should().Be("User1");
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var user = new User { Username = "User1", Password = "Password" };
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.AuthenticateUserAsync(user.Username, user.Password))
                .ReturnsAsync((User)null);

            var controller = new UserController(mockUserService.Object);

            // Act
            var result = await controller.Authenticate(user);

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
