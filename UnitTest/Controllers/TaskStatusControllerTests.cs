using Application.Interfaces.IServices.ITaskStatusServices;
using Application.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TP1_Rocio_Kreick.Controllers;
using Xunit;

namespace UnitTest.Controllers
{
    public class TaskStatusControllerTests
    {
        private readonly Mock<ITaskStatusGetServices> _mockService;
        private readonly TaskStatusController _controller;

        public TaskStatusControllerTests()
        {
            _mockService = new Mock<ITaskStatusGetServices>();
            _controller = new TaskStatusController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturn200StatusCode_WhenDataExists()
        {
            // Arrange
            var mockResponse = new List<GenericResponse>
            {
                new GenericResponse { Id = 1, Name = "Pending" },
                new GenericResponse { Id = 2, Name = "In Progress" }
            };

            _mockService.Setup(service => service.GetAll()).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<JsonResult>().Subject;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(mockResponse);
        }
    }
}
