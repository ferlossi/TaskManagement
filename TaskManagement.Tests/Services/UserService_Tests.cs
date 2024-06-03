using FluentAssertions;
using Moq;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Tests.Services
{
    public class UserService_Tests
    {
        [Fact]
        public async Task AuthenticateUserAsync_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<User> { new User { Id = 1, Username = "testuser", Password = "password" } });
            var userService = new UserService(mockRepository.Object);

            // Act
            var user = await userService.AuthenticateUserAsync("testuser", "password");

            // Assert
            user.Should().NotBeNull();
            user.Username.Should().Be("testuser");
        }

        [Fact]
        public async Task AuthenticateUserAsync_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<User>>();
            mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<User> { new User { Id = 1, Username = "testuser", Password = "password" } });
            var userService = new UserService(mockRepository.Object);

            // Act
            var user = await userService.AuthenticateUserAsync("testuser", "wrongpassword");

            // Assert
            user.Should().BeNull();
        }
    }
}
