using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using TaskManagement.Middlewares;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Tests.Middlewares
{
    public class AuthenticationMiddleware_Tests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware_Tests()
        {
            _mockUserService = new Mock<IUserService>();
            _next = (HttpContext) => Task.CompletedTask;
        }

        private AuthenticationMiddleware CreateMiddlewareInstance()
        {
            return new AuthenticationMiddleware(_next, _mockUserService.Object);
        }

        [Fact]
        public async Task InvokeAsync_ValidCredentials_ShouldCallNextMiddleware()
        {
            // Arrange
            var middleware = CreateMiddlewareInstance();
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/tasks";
            context.Request.Headers["Username"] = "testuser";
            context.Request.Headers["Password"] = "password";

            var user = new User { Id = 1, Username = "testuser", Password = "password" };
            _mockUserService.Setup(service => service.AuthenticateUserAsync("testuser", "password")).ReturnsAsync(user);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Items["User"].Should().Be(user);
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task InvokeAsync_InvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var middleware = CreateMiddlewareInstance();
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/tasks";
            context.Request.Headers["Username"] = "testuser";
            context.Request.Headers["Password"] = "wrongpassword";

            _mockUserService.Setup(service => service.AuthenticateUserAsync("testuser", "wrongpassword")).ReturnsAsync((User)null);

            var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(memoryStream).ReadToEnd();
            responseBody.Should().Be("Unauthorized: Invalid credentials.");
        }

        [Fact]
        public async Task InvokeAsync_MissingHeaders_ShouldReturnUnauthorized()
        {
            // Arrange
            var middleware = CreateMiddlewareInstance();
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/todoitem";

            var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(memoryStream).ReadToEnd();
            responseBody.Should().Be("Unauthorized: Username and Password headers are required.");
        }

        [Fact]
        public async Task InvokeAsync_NonApiPath_ShouldCallNextMiddleware()
        {
            // Arrange
            var middleware = CreateMiddlewareInstance();
            var context = new DefaultHttpContext();
            context.Request.Path = "/home";

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
