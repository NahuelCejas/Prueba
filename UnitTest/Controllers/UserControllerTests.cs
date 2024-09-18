using Application.Interfaces.IServices.IUserServices;
using Application.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TP1_Rocio_Kreick.Controllers;
using Xunit;

namespace UnitTest.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserGetServices> _mockService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockService = new Mock<IUserGetServices>();
            _controller = new UserController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfUsers()
        {
            // Arrange
            var mockResponse = new List<Users>
            {
                new Users { Id = 1, Name = "User 1", Email = "user1@gmail.com" },
                new Users { Id = 2, Name = "User 2", Email = "user2@gmail.com" }
            };

            _mockService.Setup(service => service.GetAll()).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var jsonResult = result.Should().BeOfType<JsonResult>().Subject;
            jsonResult.StatusCode.Should().Be(200);
            jsonResult.Value.Should().BeEquivalentTo(mockResponse);
        }
    }
}
